using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopwareIntegration;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = await ShopwareClient.CreateAsync().ConfigureAwait(false);
            await ExecuteRequestDemo(client, "user");
            System.Console.WriteLine("for product request press enter");
            System.Console.ReadLine();
            await ExecuteRequestDemo(client, "product?limit=2");
        }

        static async Task ExecuteRequestDemo(ShopwareClient client, string uri)
        {
            var result = await client.GetAsync(uri).ConfigureAwait(false);

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
    }
}
