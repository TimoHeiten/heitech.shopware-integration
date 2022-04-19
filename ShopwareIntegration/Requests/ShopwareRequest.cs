using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShopwareIntegration.Filter;
using ShopwareIntegration.Models.Exceptions;

namespace ShopwareIntegration.Requests
{
    ///<summary>
    /// Encapsulates the created Request to the Shopware API
    ///</summary>
    public class ShopwareRequest<T>
    {
        private Uri Uri { get; }
        private HttpMethod Method { get; }
        private IParameter? Parameter { get; }
        private bool ShouldAuthenticateRequest { get; }

        private readonly ShopwareClient _client;

        internal ShopwareRequest(ShopwareClient client,
                                 Uri uri,
                                 HttpMethod httpMethod,
                                 bool shouldAuthenticateRequest,
                                 IParameter? parameter = null)
        {
            _client = client;

            Uri = uri;
            Method = httpMethod;
            Parameter = parameter;
            ShouldAuthenticateRequest = shouldAuthenticateRequest;
        }

        public static implicit operator HttpRequestMessage(ShopwareRequest<T> rq)
        {
            HttpRequestMessage message = new()
            {
                Method = rq.Method,
                RequestUri = rq.Uri
            };

            if (rq.Parameter is not null)
                message.Content = JsonContent.Create(rq.Parameter.AsContent());

            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return message;
        }

        public Task ExecuteAsync(Action<T> onSuccess, Action<Failure> onFailure)
            => ExecuteAsync(
                model => { onSuccess(model); return Task.CompletedTask; },
                f => { onFailure(f); return Task.CompletedTask; }
            );

        public async Task ExecuteAsync(Func<T, Task> onSuccess, Func<Failure, Task> onFailure)
        {
            if (ShouldAuthenticateRequest)
                await _client.AuthenticateAsync().ConfigureAwait(false);

            async Task<RequestResult<T>> performRequestAsync()
            {
                var responseMessage = await _client.HttpClient.SendAsync(this).ConfigureAwait(false);
                return await ParseAsync(responseMessage).ConfigureAwait(false);
            }

            var rq = await performRequestAsync().ConfigureAwait(false);

            if (rq.NotAuthenticated() && _client.Configuration.CanRetry())
            {
                int start = _client.Configuration.Retries!.Value;
                while (start > 0)
                {
                    await _client.AuthenticateAsync().ConfigureAwait(false);
                    rq = await performRequestAsync().ConfigureAwait(false);
                    if (rq.IsSuccess)
                        break;
                    start--;
                }
            }

            await rq.EvalAsync(onSuccess, onFailure);
        }

        private static async Task<RequestResult<T>> ParseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    string content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    T? model = JsonConvert.DeserializeObject<T>(content!);
                    return RequestResult<T>.Success(model!);
                }
                catch (System.Exception ex)
                {
                    return RequestResult<T>.Failed(ex, false);
                }
            }
            else
            {
                bool isNotAuthenticated = httpResponse.StatusCode == HttpStatusCode.Forbidden
                                          || httpResponse.StatusCode == HttpStatusCode.Unauthorized;

                return RequestResult<T>.Failed(
                    new ShopIntegrationRequestException((int)httpResponse.StatusCode, null!, httpResponse.ReasonPhrase ?? "no reason specified"),
                    isNotAuthenticated
                );
            }
        }
    }
}
