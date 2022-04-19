using System;
using System.Text.Json.Serialization;

namespace ShopwareIntegration.Configuration
{
    ///<summary>
    /// Configuration Object for the underlying Shopware HttpClient
    ///</summary>
    public class HttpClientConfiguration
    {
        public string BaseUrl { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public int? Retries { get; set; } = null!;

        [JsonConstructor]
        public HttpClientConfiguration(string baseUrl, string clientId, string clientSecret)
            => (BaseUrl, ClientId, ClientSecret) = (baseUrl, clientId, clientSecret);

        ///<summary>
        /// Implies that retries are NOT NULL And GT 0
        ///</summary>
        internal bool CanRetry()
        {
            return Retries.HasValue && Retries.Value > 0;
        }
    }
}
