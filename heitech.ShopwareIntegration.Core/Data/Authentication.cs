using heitech.ShopwareIntegration.Core.Configuration;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core.Data
{
    ///<summary>
    /// Result Model
    ///</summary>
    internal sealed class Authenticated
    {
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    ///<summary>
    /// Request Model
    ///</summary>
    [ModelUri("oauth/token")]
    internal sealed class Authenticate
    {
        public const string Url = "oauth/token";

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        internal static Authenticate From(HttpClientConfiguration configuration) =>
            new Authenticate()
            {
                ClientId = configuration.ClientId,
                ClientSecret = configuration.ClientSecret
            };
    }
}
