using System;
using System.Net.Http;
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
            var config = new HttpClientConfiguration("api", "client", "", "secret");
            var client = await ShopwareClient.CreateAsync(config);
            
            var requestMessage = client.CreateHttpRequest("product/33090fdb7a7a4e49acd4c73b86cdddec");
            var result = await client.SendAsync<DataObject<Product>>(requestMessage );

            // var postRequest = client.CreateHttpRequest("unit", HttpMethod.Post, CreateUnit());
            // var result = await client.SendAsync<DataEmpty>(postRequest);

            if (result.IsSuccess)
                Console.WriteLine("SUCCESS!!!" + " " + result.Model + " -- " + result.Model);
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

        private static object CreateUnit()
        {
            return new
            {
                id="af1bfc368263492cae61627cc6ced9de",
                name="unit-1",
                shortCode = "shortCode",
                translated= false,
                createdAt="2022-06-04T23:56:05.804128+02:00",
                updatedAt="2022-06-04T23:56:05.819296+02:00",
                products=new object[] {} 
            };
        }
    }
}