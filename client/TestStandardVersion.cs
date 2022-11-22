using System;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using Newtonsoft.Json;
using heitech.ShopwareIntegration.Core;

namespace client
{
    public static class TestStandardVersion
    {
        public static async Task WithStandardClient()
        {
            var config = new HttpClientConfiguration("/api", "client", "user", "secret");
            var client = await ShopwareClient.CreateAsync(config);
            var requestMessage = client.CreateHttpRequest("product/33090fdb7a7a4e49acd4c73b86cdddec");

            var result = await client.SendAsync<DataObject<Product>>(requestMessage);

//todo -> fails with unsupported media type for some reason or another :/
            if (result.IsSuccess)
                Console.WriteLine("SUCCESS!!!" + " " + result.Model.Data.Id + " -- " + result.Model.Data.Description);
            else
                Console.WriteLine("Failed!!!" + " " + result.Exception.Message);
        }

        public sealed class Product
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; } = default!;
        }
    }
}