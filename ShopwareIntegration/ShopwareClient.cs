using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Exceptions;

namespace ShopwareIntegration
{
    ///<summary>
    /// HttpClient Abstraction for the Shopware REST API Integration
    ///</summary>
    public class ShopwareClient : IDisposable
    {
        private bool _disposedValue;
        internal HttpClient HttpClient;
        internal HttpClientConfiguration Configuration { get; private set; } = default!;

        private ShopwareClient(HttpClient client)
            => HttpClient = client;

        ///<summary>
        /// Create the Httpclient and the Client itself using the supplied configuration
        ///</summary>
        public static ShopwareClient Create(HttpClientConfiguration configuration)
        {
            HttpClient client = new() { BaseAddress = new Uri(configuration.BaseUrl) };
            return new(client) { Configuration = configuration };
        }

        ///<summary>
        /// Create the Httpclient and the Client itself using the configuration from the parameters
        ///</summary>
        public static ShopwareClient Create(string baseUrl, string clientId, string clientSecret)
            => Create(new(baseUrl, clientId, clientSecret));

        ///<summary>
        /// Authenticates the encapsulated Httpclient against the oauth endpoint using the current httpclient configuration
        ///</summary>
        public async Task AuthenticateAsync()
        {
            var authenticateBody = Authenticate.From(Configuration);
            var response = await HttpClient.PostAsync(Authenticate.Url, JsonContent.Create(authenticateBody)).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var authenticated = System.Text.Json.JsonSerializer.Deserialize<Authenticated>(content);
                HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticated!.AccessToken}");
            }
            else
                throw new ShopIntegrationRequestException((int)response.StatusCode, null, content);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    HttpClient.Dispose();
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
