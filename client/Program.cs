using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using heitech.ShopwareIntegration;
using heitech.ShopwareIntegration.Configuration;
using heitech.ShopwareIntegration.Models;
using heitech.ShopwareIntegration.ProductUseCases;
using heitech.ShopwareIntegration.Requests;
using ShopwareIntegration;
using ShopwareIntegration.Models;
using ShopwareIntegration.Requests;

namespace client
{
    class Program
    {
        ///<summary>
        /// Just different scenarios for testing the apis behavior
        ///</summary>
        static async Task Main(string[] args)
        {
            // test store creds
            var configuration = new HttpClientConfiguration(
                baseUrl: "http://sw6.wbv24.com/api/",
                clientId: "SWIATKTYADFGUWC2CM53VFKWBG",
                userName: string.Empty,
                clientSecret: "Nk9XQWQzSkRwVnQ2T01LTzJydnM5M3RQTFVJNW1SY3NJM3NTckY"
            );
            var client = await ShopwareClient.CreateAsync(configuration).ConfigureAwait(false);

            var readCase = new ReadUseCases(client);
            var watch = new Stopwatch();
            watch.Start();
            var result = await readCase.GetProductsPage(new ProductPaging(1, 20));
            watch.Stop();
            System.Console.WriteLine($"elapsed: {watch.ElapsedMilliseconds} ms ");

            var single = await readCase.GetProductById(result.First().Id);
            System.Console.WriteLine("single result:");
            System.Console.WriteLine(string.Join(Environment.NewLine,
                new string[] { single.Manufacturer?.Name, single.Id, single.Ean, $"{single.Stock}", $"{single.Active}", $"{single.AvailableStock}" }
            ));

            var writeUseCase = new WriteUseCase(client);
            bool wasUpdated = await writeUseCase.Update(single.Id, 4, 42.12M);
            System.Console.WriteLine($"was updated '{wasUpdated}'");

            var updated = await readCase.GetProductById(single.Id);
            System.Console.WriteLine(updated.Price.First().Gross + " - " + updated.Id + " - " + updated.Stock);
        }
    }
}
