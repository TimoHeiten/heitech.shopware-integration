using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShopwareIntegration;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Filters;

namespace client
{
    // todo
    // shopware backend -> User Administration/Settings --> Edit User && mark enabled checkbox at API Access
    //      returns API Key which si requried for authentication
    // --> " If the edited user is currently logged in, you might need to clear the backend cache, and then log out and log in for your changes to take effect."
    // ----------------------------------------------------------------------
    // Test Auth 
    // alle Entitäten als library
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = await ShopwareClient.CreateAsync();
            var addressRequest = client.CreateGetRequest<Address, int>(42);
            var result = await client.SendRequestAsync<Address>(addressRequest, CancellationToken.None);

            if (result.IsSuccess)
                System.Console.WriteLine($"Success!! - {result.Model}");
            else
                System.Console.WriteLine($"FAILED!! - {result.Exception}");

            System.Console.WriteLine();
            var filterExamples = BuildAllExampleFilters();
            System.Console.WriteLine(JsonConvert.SerializeObject(filterExamples, Formatting.Indented));
        }

        static IEnumerable<object> BuildAllExampleFilters()
        {
            var builder = new FilterBuilder();
            return builder.AddFilter("pseudoSales", value: "1", expression: ">=")
                          .AddFilter("active", value: "1")
                          .AddFilter("name", value: "%beach%", @operator: "1")
                          .AddSort(property: "name")
                          .AddSort(property: "invoiceAmount", direction: "DESC")
                          .AddLimit(limit:50, start: 20)
                          .BuildFilter();
        }
    }
}
