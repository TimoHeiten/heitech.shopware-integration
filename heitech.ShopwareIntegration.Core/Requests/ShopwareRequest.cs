using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Data;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core.Requests
{
    ///<summary>
    /// Encapsulates the actual Request to the Shopware Api and handles Authentication and Request Customization via a Builder pattern
    ///</summary>
    internal sealed class ShopwareRequest
    {
        private readonly bool _withAuth;
        private readonly ShopwareClient _client;
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly CancellationToken _cancellationToken;

        private ShopwareRequest(ShopwareClient client,
                                HttpRequestMessage httpRequestMessage,
                                bool withAuth,
                                CancellationToken cancellationToken)
        {
            _client = client;
            _withAuth = withAuth;
            _httpRequestMessage = DeepClone(httpRequestMessage);
            _cancellationToken = cancellationToken;
        }

        private static HttpRequestMessage DeepClone(HttpRequestMessage message)
        {
            var cloned = new HttpRequestMessage()
            {
                Content = message.Content,
                Method = message.Method,
                RequestUri = message.RequestUri
            };
            var headers = message.Headers;
            headers.ToList().ForEach(x => cloned.Headers.Add(x.Key, x.Value));
            return cloned;
        }

        internal async Task<Response> SendAsync()
        {
            if (_withAuth)
                await _client.AuthenticateAsync(_cancellationToken);

            try
            {
                var response = await _client.HttpClient.SendAsync(_httpRequestMessage, _cancellationToken).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return new Response
                {
                    Content = content,
                    Origin = _httpRequestMessage,
                    StatusCode = response.StatusCode,
                    IsSuccess = response.IsSuccessStatusCode
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal sealed class Response
        {
            public bool IsSuccess { get; set; }
            internal string Content { get; set; }
            internal HttpStatusCode StatusCode { get; set; }
            internal HttpRequestMessage Origin { get; set; }

            internal RequestResult<T> ParseResult<T>()
                where T : ShopwareDataContainer
            {
                // crud returns 201 No Content and cannot be Deserialized,
                // so we return early here
                if (IsCrud(Origin))
                    return RequestResult<T>.Success((T)(object)new DataEmpty());

                try
                {
                    var model = JsonConvert.DeserializeObject<T>(Content);
                    return model != null
                        ? RequestResult<T>.Success(model)
                        : RequestResult<T>.Failed(new ShopwareIntegrationRequestException(typeof(T)));
                }
                catch (Exception ex)
                {
                    return RequestResult<T>.Failed(ex);
                }
            }

            private static bool IsCrud(HttpRequestMessage msg)
            {
                var isPatch = string.Equals(msg.Method.ToString(), "Patch", StringComparison.InvariantCultureIgnoreCase);
                // post is also used for the search endpoint
                var isCreate = msg.Method == HttpMethod.Post && !msg.RequestUri.AbsoluteUri.Contains("search");
                var isDelete = msg.Method == HttpMethod.Delete;
                return isPatch || isCreate || isDelete;
            }
        }

        internal sealed class Builder
        {
            private bool _withAuth;
            private readonly ShopwareClient _client;
            private CancellationToken _withCancellation;
            private readonly HttpRequestMessage _httpRequestMessage;

            public Builder(ShopwareClient client, HttpRequestMessage httpRequestMessage)
                => (_client, _httpRequestMessage) = (client, httpRequestMessage);

            internal ShopwareRequest Build()
                => new ShopwareRequest(_client, _httpRequestMessage, _withAuth, _withCancellation);

            internal Builder WithCancellationToken(CancellationToken token)
            {
                _withCancellation = token;
                return this;
            }

            internal Builder WithNewAuth()
            {
                _withAuth = true;
                return this;
            }
        }
    }
}