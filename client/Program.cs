using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ShopwareIntegration;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Filter;
using ShopwareIntegration.Requests;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // demo shop credentials for test scenario
            string baseUri = "";
            string clientId = "";
            string clientSecrect = "";

            HttpClientConfiguration clientConfiguration = new(baseUri, clientId, clientSecrect);
            using var client = ShopwareClient.Create(clientConfiguration);

            Dictionary<string, object> filter = new() { ["limit"] = 1 };
            ShopwareRequest<Dictionary<string, object>> request = client.BuilderFromClient<Dictionary<string, object>>()
                                                                        .WithUri("search/product")
                                                                        .WithMethod(HttpMethod.Post)
                                                                        .WithParameter(new SimpleFilter(filter))
                                                                        .WithExplicitAuthentication()
                                                                        .Build();

            await request.ExecuteAsync(
                onSuccess: map => System.Console.WriteLine("success!! - " + Newtonsoft.Json.JsonConvert.SerializeObject(map).Substring(0, 50) + "..."),
                onFailure: f => System.Console.WriteLine(f.Exception)
            );

            System.Console.WriteLine();
            System.Console.WriteLine("done!");
            System.Console.ReadLine();
        }
    }
}
