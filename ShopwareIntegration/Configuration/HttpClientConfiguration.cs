using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopwareIntegration.Configuration
{
    ///<summary>
    /// Configuration Object for the underlying Shopware HttpClient
    ///</summary>
    public class HttpClientConfiguration
    {
        public string BaseUrl { get; }
        public string ClientId { get; }
        public string UserName { get; }
        public string ClientSecret { get; }

        [JsonConstructor]
        public HttpClientConfiguration(string baseUrl, string clientId, string userName, string clientSecret)
            => (BaseUrl, ClientId, UserName, ClientSecret) = (baseUrl, clientId, userName, clientSecret);


        // todo override with preferred mechanism (db, file based, user data etc.)
        public static async Task<HttpClientConfiguration?> LoadAsync()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            var text = await File.ReadAllTextAsync(filePath, Encoding.UTF8).ConfigureAwait(false);
            
            return System.Text.Json.JsonSerializer.Deserialize<HttpClientConfiguration>(text);
        }
    }
}
