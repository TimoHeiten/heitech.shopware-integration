using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using models;
using models.filters;
using Newtonsoft.Json;

namespace client
{
    // todo
    // shopware backend -> User Administration/Settings --> Edit User && mark enabled checkbox at API Access
    //      returns API Key which si requried for authentication
    // --> " If the edited user is currently logged in, you might need to clear the backend cache, and then log out and log in for your changes to take effect."
    // ----------------------------------------------------------------------
    // alle Entitäten als library
    // Test Auth 
    // sende an meine eigene API
    // 
    class Program
    {
        static HttpClient _client;
        static async Task Main(string[] args)
        {
            var config = await HttpClientConfiguration.LoadAsync();

            var token = CancellationToken.None;

            _client = PrepareHttpClient(config);
            var uri = new Uri($"{_client.BaseAddress}{Endpoints.Address.Get(1)}");
            // System.Console.WriteLine(uri);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri,
                Content = JsonContent.Create(FilterObject.Empty)
            };

            var response = await _client.SendAsync(request);

            System.Console.WriteLine($"{response.StatusCode} from get");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine(content);
                var address = JsonConvert.DeserializeObject<Address>(content);
                System.Console.WriteLine(address.Id + " " + address.Customer + $" {address.FirstName} {address.LastName}");
            }
        }

        static HttpClient PrepareHttpClient(HttpClientConfiguration configuration)
        {
            HttpClientHandler trustingHandler = new();
            trustingHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new(trustingHandler) { BaseAddress = new Uri(configuration.BaseUrl) };

            client.DefaultRequestHeaders.Add("Authorization", configuration.CreateBasicAuthHeader());
            client.DefaultRequestHeaders.Add("ApiKey", configuration.ApiKey);

            return client;
        }
    }
}
