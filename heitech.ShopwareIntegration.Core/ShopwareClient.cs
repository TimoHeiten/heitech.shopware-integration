using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Requests;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core
{
    ///<summary>
    /// HttpClient Abstraction for the Shopware REST API Integration.
    /// <para> Be careful with the Dispose, since it uses an httpclient underneath and can lead to Socketexhaustion on lots of create/dispose calls to this client</para>
    ///</summary>
    public sealed class ShopwareClient : IDisposable
    {
        private bool _disposedValue;
        internal HttpClient HttpClient { get; }
        internal HttpClientConfiguration Configuration { get; set; }
        internal string BaseUrl => Configuration.BaseUrl;

        internal ShopwareClient(HttpClient client)
            => HttpClient = client;

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

            if (!clientConfiguration.IsValid())
                throw new ShopwareIntegrationRequestException($"{nameof(HttpClientConfiguration)} is not valid! Check if all Properties are set appropriately (non empty and valid)");

            var httpClient = clientConfiguration.CreateHttpClient();
            var shopwareClient = new ShopwareClient(httpClient)
            {
                Configuration = clientConfiguration
            };
            await shopwareClient.AuthenticateAsync().ConfigureAwait(false);

            return shopwareClient;
        }

        /// <summary>
        /// Try to authenticate against the Shopware API
        /// </summary>
        /// <exception cref="ShopwareIntegrationRequestException"></exception>
        public async Task AuthenticateAsync()
        {
            var authenticateBody = Authenticate.From(Configuration);
            var response = await HttpClient.PostAsync(Authenticate.Url, authenticateBody.AsJsonContent()).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var authenticated = JsonConvert.DeserializeObject<Authenticated>(content);
                if (authenticated.AccessToken is null)
                    throw new ShopwareIntegrationRequestException($"Authentication did nor result in a success.{Environment.NewLine}Content: '{content}'");

                HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticated?.AccessToken}");
            }
            else
            {
                throw new ShopwareIntegrationRequestException((int)response.StatusCode, null, content);
            }
        }

        /// <summary>
        /// For fine grained control of the Inputs/Outputs use this method. In all other Cases use the SendMessage or higher level abstractions of the Crud Package
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetResponseContentAsync(HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Send a HttpRequestMessage. Includes retries and authenticates automatically. Handles different HttpMethods and the necessary Serialization
        /// </summary>
        /// <param name="message"></param>
        /// <param name="guardRecursion"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<RequestResult<T>> SendAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken = default)
            where T : ShopwareDataContainer
        {
            var builder = new ShopwareRequest.Builder(this, message).WithCancellationToken(cancellationToken);
            var response = await builder.Build()
                                        .SendAsync()
                                        .ConfigureAwait(false);

            // retry on failed authentication
            if (response is null || response.IsSuccess is false)
            {
                var isOtherErrorThanAuth = response is null || (response.StatusCode != System.Net.HttpStatusCode.Forbidden && response.StatusCode != System.Net.HttpStatusCode.Unauthorized);
                if (isOtherErrorThanAuth)
                    return RequestResult<T>.Failed(new ShopwareIntegrationRequestException(response is null ? -1 : (int)response.StatusCode, message, response?.Content ?? "no content was found!"));

                try
                {
                    response = await builder.WithNewAuth()
                                            .Build()
                                            .SendAsync()
                                            .ConfigureAwait(false);

                    if (response is null || response.IsSuccess is false)
                        throw new ShopwareIntegrationRequestException($"Request failed: '{response.Content}'");
                }
                catch (Exception ex)
                {
                    return RequestResult<T>.Failed(ex);
                }
            }

            return response.ParseResult<T>();
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;

            if (disposing)
                HttpClient.Dispose();

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
