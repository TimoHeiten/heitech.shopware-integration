using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    // todo
    // shopware backend -> User Administration/Settings --> Edit User && mark enabled checkbox at API Access
    //      returns API Key which si requried for authentication
    // --> " If the edited user is currently logged in, you might need to clear the backend cache, and then log out and log in for your changes to take effect."
    // ----------------------------------------------------------------------
    // 
    class Program
    {
        static HttpClient _client;
        static async Task Main(string[] args)
        {
            var config = await HttpClientConfiguration.LoadAsync();

            _client = PrepareHttpClient(config);
        }

        static HttpClient PrepareHttpClient(HttpClientConfiguration configuration)
        {
            HttpClient client = new() { BaseAddress = new Uri(configuration.BaseUrl) };

            client.DefaultRequestHeaders.Add("Authorization", configuration.CreateBasicAuthHeader());
            client.DefaultRequestHeaders.Add("ApiKey", configuration.ApiKey);

            return client;
        }
    }
}
