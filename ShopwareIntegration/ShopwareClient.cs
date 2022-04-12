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
            await shopwareClient.AuthenticateAsync().ConfigureAwait(false);

            return shopwareClient;
        }

        public Task<RequestResult<Dictionary<string, object>>> GetAsync(string uri)
            => SendAsync(CreateHttpRequest(uri));

        public Task<RequestResult<Dictionary<string, object>>> PostAsync<T>(string uri, T model)
            => SendAsync(CreateHttpRequest<T>(uri, model));

        public Task<RequestResult<Dictionary<string, object>>> PutAsync<T>(string uri, T model)
            => SendAsync(CreateHttpRequest<T>(uri, model, HttpMethod.Put));

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

        public async Task<string> GetResponseContentAsync(HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            var response = await _client.SendAsync(message, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<RequestResult<Dictionary<string, object>>> SendAsync(HttpRequestMessage message, bool? guardRecursion = false, CancellationToken cancellationToken = default)
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
