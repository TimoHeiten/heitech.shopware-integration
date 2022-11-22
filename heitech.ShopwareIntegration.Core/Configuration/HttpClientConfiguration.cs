using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core.Configuration
{
    ///<summary>
    /// Configuration Object for the underlying Shopware HttpClient
    ///</summary>
    public class HttpClientConfiguration
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string ClientSecret { get; set; }

        public HttpClientConfiguration()
        { }

        public HttpClientConfiguration(string baseUrl, string clientId, string userName, string clientSecret)
            => (BaseUrl, ClientId, UserName, ClientSecret) = (baseUrl, clientId, userName, clientSecret);

        ///<summary>
        /// Provide a Stream that holds the Configuration in JSON Format to Deserialize into an instance of the HttpClientConfiguration
        ///</summary>
        public static async Task<HttpClientConfiguration> LoadAsync(Stream stream)
        {
            using (var stringReader = new StreamReader(stream))
            {
                var contents = await stringReader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<HttpClientConfiguration>(contents);
            }
        }

        ///<summary>
        /// Provide a FileInfo to a configuration file in JSON Format to Deserialize into an instance of the HttpClientConfiguration
        ///</summary>
        public static Task<HttpClientConfiguration> LoadAsync(FileInfo fileInfo)
            => LoadAsync(File.OpenRead(fileInfo.FullName));

        internal virtual HttpClient CreateHttpClient()
            => new HttpClient() 
            {
                BaseAddress = new Uri(this.BaseUrl)
            };

        internal bool IsValid()
        {
            var uriIsValid = Uri.TryCreate(BaseUrl, UriKind.RelativeOrAbsolute, out var _);
            var clientIdIsValid = !string.IsNullOrWhiteSpace(ClientId);
            var clientSecretIsValid = !string.IsNullOrWhiteSpace(ClientSecret);
            var userNameExists = !string.IsNullOrWhiteSpace(UserName);

            return uriIsValid && clientIdIsValid && clientSecretIsValid && userNameExists;
        }
    }
}
