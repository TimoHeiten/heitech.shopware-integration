using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Configuration;
using heitech.ShopwareIntegration.Models;
using heitech.ShopwareIntegration.Requests;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Exceptions;
using ShopwareIntegration.Requests;

namespace heitech.ShopwareIntegration
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

        public static async Task<ShopwareClient> CreateAsync(Configuration.HttpClientConfiguration clientConfiguration)
        {
            var configuration = clientConfiguration;
            if (configuration is null)
                throw new NullReferenceException($"The HttpConfiguration could not be loaded at: {typeof(ShopwareClient)} {nameof(CreateAsync)}");

            HttpClient client = new() { BaseAddress = new Uri(configuration.BaseUrl) };

            ShopwareClient shopwareClient = new(client) { _configuration = configuration };
            await shopwareClient.AuthenticateAsync().ConfigureAwait(false);

            return shopwareClient;
        }

        public Task<RequestResult<T>> GetAsync<T>(string uri)
            => SendAsync<T>(CreateHttpRequest(uri));

        public Task<RequestResult<T>> PostAsync<T>(string uri, T model)
            => SendAsync<T>(CreateHttpRequest<T>(uri, model));

        public Task<RequestResult<T>> PutAsync<T>(string uri, T model)
            => SendAsync<T>(CreateHttpRequest<T>(uri, model, HttpMethod.Put));

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

        public async Task<string> GetResponseContentAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            var response = await _client.SendAsync(message, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<RequestResult<T>> SendAsync<T>(HttpRequestMessage message, bool? guardRecursion = false, CancellationToken cancellationToken = default)
        {
            var response = await _client.SendAsync(message, cancellationToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode is false)
            {
                var code = response.StatusCode;
                if (code == System.Net.HttpStatusCode.Forbidden || code == System.Net.HttpStatusCode.Unauthorized)
                {
                    try
                    {
                        await AuthenticateAsync().ConfigureAwait(false);

                        if (guardRecursion.HasValue && guardRecursion.Value)
                            return RequestResult<T>.Failed(new ShopIntegrationRequestException(typeof(T)));

                        await SendAsync<T>(message, guardRecursion: true, cancellationToken: cancellationToken);
                    }
                    catch (System.Exception ex)
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
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                return RequestResult<T>.Failed(ex);
            }
        }

        private bool IsCrud(HttpRequestMessage msg)
        {
            bool isPatch = msg.Method == HttpMethod.Patch;
            // post is also used for the search endpoint
            bool isCreate = msg.Method == HttpMethod.Post && !msg.RequestUri!.AbsoluteUri.Contains("search");
            bool isDelete = msg.Method == HttpMethod.Delete;
            return isPatch || isCreate || isDelete;
        }

        public ReadRequest<T> CreateReader<T>() where T : BaseEntity => new ReadRequest<T>(this);

        public WritingRequest<T> CreateWriter<T>() where T : BaseEntity => new WritingRequest<T>(this);


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
