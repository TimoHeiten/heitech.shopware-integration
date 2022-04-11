using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Exceptions;
using ShopwareIntegration.Requests;

namespace ShopwareIntegration
{
    ///<summary>
    /// HttpClient Abstraction for the Shopware REST API Integration
    ///</summary>
    public class ShopwareClient : IDisposable
    {
        private bool _disposedValue;
        private readonly HttpClient _client;
        private HttpClientConfiguration _configuration = default!;

        private ShopwareClient(HttpClient client)
            => _client = client;

        public static async Task<ShopwareClient> CreateAsync()
        {
            var configuration = await Configuration.HttpClientConfiguration.LoadAsync().ConfigureAwait(false);
            if (configuration is null)
                throw new NullReferenceException($"The HttpConfiguration could not be loaded at: {typeof(ShopwareClient)} {nameof(CreateAsync)}");

            HttpClient client = new() { BaseAddress = new Uri(configuration.BaseUrl) };

            ShopwareClient shopwareClient = new(client) { _configuration = configuration };
            await shopwareClient.AuthenticateAsync();

            return shopwareClient;
        }

        public HttpRequestMessage CreateHttpRequest<TModel>(string uri, TModel model, HttpMethod? method = null)
        {
            HttpRequestMessage message = new()
            {
                Content = JsonContent.Create(model),
                Method = method ?? HttpMethod.Post,
                RequestUri = new Uri($"{_configuration.BaseUrl}{uri}")
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return message;
        }

        public HttpRequestMessage CreateHttpRequest(string uri, HttpMethod? method = null, HttpContent? content = null)
        {
            HttpRequestMessage message = new()
            {
                Content = content,
                Method = method ?? HttpMethod.Get,
                RequestUri = new Uri($"{_configuration.BaseUrl}{uri}")
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return message;
        }

        public async Task AuthenticateAsync()
        {
            var authenticateBody = Authenticate.From(_configuration);
            var response = await _client.PostAsync(Authenticate.Url, JsonContent.Create(authenticateBody));
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var authenticated = System.Text.Json.JsonSerializer.Deserialize<Authenticated>(content);
                System.Console.WriteLine(authenticated!.AccessToken);
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticated!.AccessToken}");
            }
            else
                throw new ShopIntegrationRequestException((int)response.StatusCode, null, content);
        }

        public async Task<string> GetResponseContentAsync(HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            var response = await _client.SendAsync(message, cancellationToken);
            return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<RequestResult<Dictionary<string, object>>> SendAsync(HttpRequestMessage message, bool? guardRecursion = false, CancellationToken cancellationToken = default)
        {
            var response = await _client.SendAsync(message, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode is false)
            {
                if (response.StatusCode.ToString().StartsWith("4")) // forbidden or not authorized, then try to authenticate again and retry request
                {
                    try
                    {
                        await AuthenticateAsync();

                        if (guardRecursion.HasValue && guardRecursion.Value)
                            return RequestResult<Dictionary<string, object>>.Failed(new ShopIntegrationRequestException(typeof(Dictionary<string, object>)));

                        await SendAsync(message, true, cancellationToken);
                    }
                    catch (System.Exception ex)
                    {
                        return RequestResult<Dictionary<string, object>>.Failed(ex);
                    }
                }
                else
                    return RequestResult<Dictionary<string, object>>.Failed(new ShopIntegrationRequestException((int)response.StatusCode, message, content));
            }
            try
            {
                Dictionary<string, object>? model = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(content);
                return model is not null
                       ? RequestResult<Dictionary<string, object>>.Success(model!)
                       : RequestResult<Dictionary<string, object>>.Failed(new ShopIntegrationRequestException(typeof(Dictionary<string, object>)));
            }
            catch (System.Exception ex)
            {
                return RequestResult<Dictionary<string, object>>.Failed(ex);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
