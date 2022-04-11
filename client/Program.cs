using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ShopwareIntegration;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = await ShopwareClient.CreateAsync();
            await ExecuteRequestDemo(client, client.CreateHttpRequest("user"));
            System.Console.WriteLine("for product request press enter");
            System.Console.ReadLine();
            await ExecuteRequestDemo(client, client.CreateHttpRequest("product?limit=2"));
        }

        static async Task ExecuteRequestDemo(ShopwareClient client, HttpRequestMessage httpRequest)
        {
            var result = await client.SendAsync(httpRequest);


            if (result.IsSuccess)
            {
                DataContainer container = new(result.Model);
                System.Console.WriteLine($"Success!! - {container}");
            }
            else
                System.Console.WriteLine($"FAILED!! - {result.Exception}");
        }

        public class DataContainer
        {
            private readonly Dictionary<string, object> _data;
            public DataContainer(Dictionary<string, object> data)
                => _data = data;

            public override string ToString()
                => string.Join(Environment.NewLine, _data.Select(x => $"{x.Key} - {x.Value}"));
        }

        // public class ProductsResult
        // {
        //     [JsonPropertyName("data")]
        //     public IEnumerable<Product> Data { get; set; }

        //     public override string ToString()
        //     {
        //         return string.Join(", ", Data);
        //     }
        // }

        // public class Product
        // {
        //     [JsonPropertyName("id")]
        //     public string Id { get; set; }

        //     [JsonPropertyName("type")]
        //     public string Type { get; set; }

        //     [JsonPropertyName("attributes")]
        //     public object Attributes { get; set; }

        //     [JsonPropertyName("links")]
        //     public object Links { get; set; }
           
        //     [JsonPropertyName("relationships")]
        //     public object RelationShips { get; set; }

        //     [JsonPropertyName("meta")]
        //     public object Meta { get; set; }

        //     public override string ToString()
        //         => $"Id:'{Id}' - Type:'{Type}'";
        // }
    }
}
