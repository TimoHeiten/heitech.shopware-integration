using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopwareIntegration;
using ShopwareIntegration.Requests;
using ShopwareIntegration.Models;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // test store creds
            var configuration = new ShopwareIntegration.Configuration.HttpClientConfiguration(
                baseUrl: "http://sw6.wbv24.com/api/",
                clientId: "SWIATKTYADFGUWC2CM53VFKWBG",
                userName: string.Empty,
                clientSecret: "Nk9XQWQzSkRwVnQ2T01LTzJydnM5M3RQTFVJNW1SY3NJM3NTckY"
            );
            var client = await ShopwareClient.CreateAsync(configuration).ConfigureAwait(false);

            var liste = new ReadRequest<CustomerGroup>(client);
            // var dic = await liste.ExecuteListAsync();
            // System.Console.WriteLine("next");
            // System.Console.WriteLine();
            // System.Console.ReadLine();
            // dynamic d = dic.Data;
            // var item = d[0];
            // System.Console.WriteLine(item);
            _ = await liste.ExecuteGetAsync("713d0f385267496e9629564d314c1ec4");
            System.Console.WriteLine("next");
            System.Console.WriteLine();
            System.Console.ReadLine();

            // associations
            var inner = new { limit = 3 };
            var ass = new { customers = inner };
            _ = await liste.SearchAsync(new { associations = ass });
            _ = await liste.SearchIdsAsync(new { associations = ass });
        }

        static async Task ExecuteRequestDemo(ShopwareClient client, string uri)
        {
            var result = await client.GetAsync<Product>(uri).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                var container = result.Model;
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
    }
}
