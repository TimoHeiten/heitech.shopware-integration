using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using heitech.ShopwareIntegration;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.Configuration;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.StateManagerUtilities;

namespace client
{
    public static class Program
    {
        ///<summary>
        /// Just different scenarios for testing the apis behavior
        ///</summary>
        static async Task Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarked>();
            return;
            // test store creds
            var configuration = new HttpClientConfiguration(
                baseUrl: "http://sw6.wbv24.com/api/",
                clientId: "SWIATKTYADFGUWC2CM53VFKWBG",
                userName: string.Empty,
                clientSecret: "Nk9XQWQzSkRwVnQ2T01LTzJydnM5M3RQTFVJNW1SY3NJM3NTckY"
            );
            var stateManager = await WithOutCache(configuration);
            await MultiplePagesSequentially(stateManager);
            await MultiplePagesConcurrently(stateManager);
            return;
            
            var filter = new {
                limit = 100,
                page = 1
            };
            
            var pageContext = DataContext.GetPage<CategoryDetails>(1);
            pageContext.SetFilter(filter);
            var pageResult = await stateManager.RetrievePage<CategoryDetails>(pageContext);
            // var firstId = pageResult.First().Id;

            // var detailsContext = DataContext.GetDetail<CategoryDetails>(firstId, filter.page);
            // var details = await stateManager.RetrieveDetails<CategoryDetails>(detailsContext);
            // System.Console.WriteLine(JsonSerializer.Serialize(details));
            //
            // var deleteContext = DataContext.Delete<CategoryDetails>(pageResult.Last().Id, filter.page);
            // var result = await stateManager.DeleteAsync<CategoryDetails>(deleteContext);
            // System.Console.WriteLine(result?.Id);
            //
            // System.Console.WriteLine(pageResult.Count());

            await DoUpdate(stateManager, pageResult.First());
        }

        static Task<IStateManager> WithCache(HttpClientConfiguration httpClientConfiguration) =>
            Factory.CreateAsync(httpClientConfiguration);

        internal static async Task<IStateManager> WithOutCache(HttpClientConfiguration httpClientConfiguration)
        {
            var shopwareClient = await ShopwareClient.CreateAsync(httpClientConfiguration);
            return await Factory.CreateAsync(httpClientConfiguration, client: new Client(shopwareClient), logger: Factory.SilentLogger()); 
        }


        static async Task DoUpdate(IStateManager stateManager, CategoryDetails currentOne)
        {
            var patchedValues = PatchedValue.From(currentOne,
                new
                {
                    active = true,
                    name = "back to original"
                });
            var context = DataContext.Update(patchedValues, 0);
            var result = await stateManager.UpdateAsync<CategoryDetails>(context);
            Console.WriteLine(result.Active + " " + result.Name);
        }

        internal static object ProductFilter(int pageNo) => new
        {
            limit = 100,
            page = pageNo,
            // further performance improvements by limiting the fields that get pulled from the API
            includes = new IncludesFields.Product(
                "name", "price", "id", "active", "availableStock", "description", "ean",
                "stock", "manufacturerId"
            )
        };


        static async Task MultiplePagesSequentially(IStateManager manager)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < 5; i++)
            {
                var context = DataContext.GetPage<ProductDetails>(i+1);
                context.SetFilter(ProductFilter(i+1));
                _ = await manager.RetrievePage<ProductDetails>(context);
                // do nothing with result, just for (lazily written perf benchmark)
            }
            stopWatch.Stop();
            Console.WriteLine($"sequential took {stopWatch.ElapsedMilliseconds} ms");
        }

        static async Task MultiplePagesConcurrently(IStateManager manager)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var descr = new PageQueryDescription[]
            {
                new(1, ProductFilter(1)),
                new(2, ProductFilter(2)),
                new(3, ProductFilter(3)),
                new(5, ProductFilter(4)),
                new(4, ProductFilter(5)),
            };
            await manager.GetMultiplePagesConcurrently<ProductDetails>(descr);
            stopWatch.Stop();
            Console.WriteLine($"parallel took {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
