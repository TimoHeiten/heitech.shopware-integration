using System.Net.Http.Headers;
using System.Net.Http.Json;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using heitech.ShopwareIntegration.State.Integration.Models;
using heitech.ShopwareIntegration.State.Integration.Models.Data;
using heitech.ShopwareIntegration.State.Integration.Models.Exceptions;
using heitech.ShopwareIntegration.State.Integration.Requests;

namespace heitech.ShopwareIntegration.State.Integration;

///<summary>
/// HttpClient Abstraction for the Shopware REST API Integration
///</summary>
public sealed class ShopwareClient : IDisposable
{
    private bool _disposedValue;
    private readonly HttpClient _client;
    private HttpClientConfiguration _configuration = default!;

    internal string BaseUrl => _configuration.BaseUrl;

    private ShopwareClient(HttpClient client)
        => _client = client;

    /// <summary>
    /// Create a new authenticated Instance by using the specified HttpClientConfiguration 
    /// </summary>
    /// <param name="clientConfiguration"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static async Task<ShopwareClient> CreateAsync(HttpClientConfiguration clientConfiguration)
    {
        if (clientConfiguration is null)
            throw new NullReferenceException($"The HttpConfiguration could not be loaded at: {typeof(ShopwareClient)} {nameof(CreateAsync)}");

        HttpClient client = new() { BaseAddress = new Uri(clientConfiguration.BaseUrl) };

        ShopwareClient shopwareClient = new(client) { _configuration = clientConfiguration };
        await shopwareClient.AuthenticateAsync().ConfigureAwait(false);

        return shopwareClient;
    }

    /// <summary>
    /// Try to authenticate against the Shopware API
    /// </summary>
    /// <exception cref="ShopIntegrationRequestException"></exception>
    public async Task AuthenticateAsync()
    {
        var authenticateBody = Authenticate.From(_configuration);
        var response = await _client.PostAsync(Authenticate.Url, JsonContent.Create(authenticateBody)).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var authenticated = System.Text.Json.JsonSerializer.Deserialize<Authenticated>(content);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticated!.AccessToken}");
        }
        else
            throw new ShopIntegrationRequestException((int)response.StatusCode, null, content);
    }

    /// <summary>
    /// For fine grained control of the Inputs/Outputs use this method. In all other Cases use the SendMessage or higher level abstractions.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> GetResponseContentAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken = default)
    {
        var response = await _client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a HttpRequestMessage. Includes retries and authenticates automatically. Sorts for different HttpMethods and the necessary serializations
    /// </summary>
    /// <param name="message"></param>
    /// <param name="guardRecursion"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<RequestResult<T>> SendAsync<T>(HttpRequestMessage message, bool? guardRecursion = false, CancellationToken cancellationToken = default)
    {
        var response = await _client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        // retry on failed authentication
        if (response.IsSuccessStatusCode is false)
        {
            var code = response.StatusCode;
            if (code is System.Net.HttpStatusCode.Forbidden or System.Net.HttpStatusCode.Unauthorized)
            {
                try
                {
                    await AuthenticateAsync().ConfigureAwait(false);

                    if (guardRecursion.HasValue && guardRecursion.Value)
                        return RequestResult<T>.Failed(new ShopIntegrationRequestException(typeof(T)));

                    await SendAsync<T>(message, guardRecursion: true, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    return RequestResult<T>.Failed(ex);
                }
            }
            else
                return RequestResult<T>.Failed(new ShopIntegrationRequestException((int)response.StatusCode, message, content));
        }

        // crud returns 201 No Content and cannot be Deserialized,
        // so we return early here
        if (IsCrud(message)) return RequestResult<T>.Success((T)(object)new DataEmpty());

        try
        {
            var model = System.Text.Json.JsonSerializer.Deserialize<T>(content);
            return model is not null
                ? RequestResult<T>.Success(model!)
                : RequestResult<T>.Failed(new ShopIntegrationRequestException(typeof(T)));
        }
        catch (Exception ex)
        {
            return RequestResult<T>.Failed(ex);
        }
    }

    private static bool IsCrud(HttpRequestMessage msg)
    {
        var isPatch = msg.Method == HttpMethod.Patch;
        // post is also used for the search endpoint
        var isCreate = msg.Method == HttpMethod.Post && !msg.RequestUri!.AbsoluteUri.Contains("search");
        var isDelete = msg.Method == HttpMethod.Delete;
        return isPatch || isCreate || isDelete;
    }


    private void Dispose(bool disposing)
    {
        if (_disposedValue) 
            return;

        if (disposing)
            _client.Dispose();

        _disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}