using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopwareIntegration.Configuration
{
    public class HttpClientConfiguration
    {
        public string BaseUrl { get; }
        public string ApiKey { get; }
        public string UserName { get; }
        public string Password { get; }

        [JsonConstructor]
        public HttpClientConfiguration(string baseUrl, string apiKey, string userName, string password)
            => (BaseUrl, ApiKey, UserName, Password) = (baseUrl, apiKey, userName, password);

        public string CreateBasicAuthHeader()
        {
            //Header
            /*
                Combine username and password with a single colon (:).
                Encode the string into an octet sequence (aka utf8 byte array)
                Encode the string with Base64.
                Prepend the authorization method and a space to the encoded string.
            */
            string combined = $"{UserName}:{Password}";
            byte[] asOctet = Encoding.UTF8.GetBytes(combined);
            string asBase64 = Convert.ToBase64String(asOctet);

            const string authMethod = "Basic";

            return $"{authMethod} {asBase64}";
        }

        // todo override with preferred mechanism (db, file based, user data etc.)
        public static async Task<HttpClientConfiguration?> LoadAsync()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            var text = await File.ReadAllTextAsync(filePath, Encoding.UTF8).ConfigureAwait(false);
            
            return System.Text.Json.JsonSerializer.Deserialize<HttpClientConfiguration>(text);
        }
    }
}
