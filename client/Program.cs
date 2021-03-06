using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using heitech.ShopwareIntegration;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.Models;
using heitech.ShopwareIntegration.Configuration;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Api;
using System.Text.Json;

namespace client
{
    public static class Program
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

            var stateManager = await Factory.CreateAsync(configuration);
            
            var filter = new {
                limit = 100,
                page = 1
            };
            
            var pageContext = DataContext.GetPage<CategoryDetails>(1);
            pageContext.SetFilter(filter);
            var pageResult = await stateManager.RetrievePage<CategoryDetails>(pageContext);
            var firstId = pageResult.First().Id;

            var detailsContext = DataContext.GetDetail<CategoryDetails>(firstId, filter.page);
            var details = await stateManager.RetrieveDetails<CategoryDetails>(detailsContext);
            System.Console.WriteLine(JsonSerializer.Serialize(details));

            var deleteContext = DataContext.Delete<CategoryDetails>(pageResult.Last().Id, filter.page);
            var result = await stateManager.DeleteAsync<CategoryDetails>(deleteContext);
            System.Console.WriteLine(result?.Id);

            System.Console.WriteLine(pageResult.Count());
        }
    }
}
