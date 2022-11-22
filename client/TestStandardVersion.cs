using System.Net.Http;
using System;
using System.Linq;
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
            var config = new HttpClientConfiguration("http://sw6.wbv24.com/api/",
                                                      clientId: "SWIATKTYADFGUWC2CM53VFKWBG",
                                                      userName: "erp",
                                                      clientSecret: "Nk9XQWQzSkRwVnQ2T01LTzJydnM5M3RQTFVJNW1SY3NJM3NTckY");
            var client = await ShopwareClient.CreateAsync(config);
            var requestMessage = Extensions.CreateHttpRequest(client, "product/33090fdb7a7a4e49acd4c73b86cdddec");

            var result = await client.SendAsync<DataObject<Product>>(requestMessage);

//todo -> fails with unsupported media type for some reason or another :/
            if (result.IsSuccess)
                System.Console.WriteLine("SUCESS!!!" + " " + result.Model.Data.Id + " -- " + result.Model.Data.Description);
            else
                System.Console.WriteLine("Failed!!!" + " " + result.Exception.Message);
        }

        public sealed class Product
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("description")]
            public string? Description { get; set; } = default!;
        }
    }
}