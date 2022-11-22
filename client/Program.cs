using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.DetailModels.Media;
using heitech.ShopwareIntegration.State.Integration;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;
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
            await TestStandardVersion.WithStandardClient();
            return;

            // BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarked>();
            // return;
            // test store creds
            var configuration = new HttpClientConfiguration(
                baseUrl: "",
                clientId: "",
                userName: string.Empty,
                clientSecret: ""
            );
            var stateManager = await WithOutCache(configuration);
            // await MultiplePagesSequentially(stateManager);
            // await MultiplePagesConcurrently(stateManager);
            // return;

            var filter2 = new {
                limit = 5,
                page = 1
            };

            await FindByEan(stateManager);

            await SetCategoryOnProduct(stateManager);
            return;

            var pageContext2 = DataContext.GetPage<ProductDetails>(1);
            pageContext2.SetFilter(filter2);
            var pageResult2 = await stateManager.RetrievePage<ProductDetails>(pageContext2);
            
            var exists = await DoesProductExist(stateManager, pageResult2.First().Id);
            Debug.Assert(exists);
            return;
            await CreateProduct(stateManager);

            // await Upload(stateManager);
            // return;
            
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

            Console.WriteLine(pageResult.First().Name);
            await DoUpdate(stateManager, pageResult.First());
        }

        static async Task FindByEan(IStateManager stateManager)
        {
            var searchByEanFilter = new
            {
                filter = new[]
                {
                    new
                    {
                        type = "equals",
                        field = "product.ean",
                        value = "4014549148099"

                    }
                }
            };

            var additionalData = new Dictionary<string, object>();
            additionalData.Add("search", searchByEanFilter);
            var context = DataContext.GetDetail<ProductDetails>("id does not matter here", 1, additionalData);

            var product = await stateManager.RetrieveDetails<ProductDetails>(context);
            
            Debug.Assert(product.Ean == searchByEanFilter.filter[0].value);
        }

        static Task<IStateManager> WithCache(HttpClientConfiguration httpClientConfiguration) =>
            Factory.CreateAsync(httpClientConfiguration);

        internal static async Task<IStateManager> WithOutCache(HttpClientConfiguration httpClientConfiguration)
        {
            var shopwareClient = await ShopwareClient.CreateAsync(httpClientConfiguration);
            return await Factory.CreateAsync(httpClientConfiguration, client: new Client(shopwareClient), logger: Factory.SilentLogger()); 
        }
        
        private static async Task<bool> DoesProductExist(IStateManager stateManager, string productId) // if so it is an update
        {
            var details = DataContext.GetDetail<ProductDetails>(productId, 1);
            // details.SetFilter(new {  includes = new IncludesFields.Product("id") });
            var result = await stateManager.RetrieveDetails<ProductDetails>(details);
            

            var update = PatchedValue.From(result, new {unitId = "46d1e688b6b843dcb4dcbbcc6f80cede" });
            var updateContext = DataContext.Update(update, 1);
            var updated = await stateManager.UpdateAsync<ProductDetails>(updateContext);

            details = DataContext.GetDetail<ProductDetails>(productId, 1);
            result = await stateManager.RetrieveDetails<ProductDetails>(details);

            return result != null;
        }


        static async Task DoUpdate(IStateManager stateManager, CategoryDetails currentOne)
        {
            var patchedValues = PatchedValue.From(currentOne,
                new
                {
                    active = true,
                    name = "this time the update is already incorporated"
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
            for (var i = 0; i < 3; i++)
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
            };
            await manager.GetMultiplePagesConcurrently<ProductDetails>(descr);
            stopWatch.Stop();
            Console.WriteLine($"parallel took {stopWatch.ElapsedMilliseconds} ms");
        }

        static async Task CreateProduct(IStateManager stateManager)
        {
            var product = new PrdDtls()
            {
                Name = "test-" + Guid.NewGuid().ToString("N"),
                ProductNumber = Guid.NewGuid().ToString("N"),
                Stock = 10,
                Price = new[]
                {
                    new
                    {
                        currencyId = "b7d2554b0ce847cd82f3ac9bd1c0dfca",
                        net = 10,
                        gross = 15,
                        linked = false
                    }
                },
                Tax = new
                {
                    name = "test",
                    taxRate = 15
                }
            };
            var context = DataContext.Create(product, 1);
            var result = await stateManager.CreateAsync<PrdDtls>(context);

            Console.WriteLine(result);

            var getDetails = DataContext.GetDetail<PrdDtls>(result.Id, 1);
            var fromGet = await stateManager.RetrieveDetails<PrdDtls>(getDetails);
            Debug.Assert(fromGet.Id == result.Id);
            Debug.Assert(fromGet.Name == result.Name);
        }

        [ModelUri("product")]
        private sealed class PrdDtls : DetailsEntity
        {
            [JsonPropertyName("name")]
            public string Name { get; init; }
            
            [JsonPropertyName("productNumber")]
            public string ProductNumber { get; init; }
            
            [JsonPropertyName("stock")]
            public int Stock { get; init; }
            
            [JsonPropertyName("price")]
            public object[] Price { get; init; }
            
            [JsonPropertyName("tax")]
            public object Tax { get; init; }
        }

        static async Task Upload(IStateManager stateManager)
        {
            var filter = new
            {
                limit = 1,
                page = 1
            };
            var dataContext = DataContext.GetPage<ProductDetails>(1);
            dataContext.SetFilter(filter);
            var pages = await stateManager.RetrievePage<ProductDetails>(dataContext);
            var productId = pages.First().Id;
            try
            {
                var fileInfo = new FileInfo("TODO:PUT YOUR PATH HERE FOR MANUAL TEST");
                var mediaId = await stateManager.InsertProductMediaFileForProductAsync(productId, fileInfo);
                Console.WriteLine(mediaId.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task SetCategoryOnProduct(IStateManager stateManager)
        {
            var productId = "001687e84c0f4b238acd3b8ed31beef5";
            var details = DataContext.GetDetail<ProductDetails>(productId, 1);
            var result = await stateManager.RetrieveDetails<ProductDetails>(details);

            var update = PatchedValue.From(result, new
            {
                categories = new []
                {
                    new
                    {
                        id = "5e3ea02f48fb42228fc355856f82d053"
                    }, 
                    new
                    {
                        id = "0084e93025484a8db0cf41c364dc0be9"
                    }
                }
            });
            var updateContext = DataContext.Update(update, 1);
            var updated = await stateManager.UpdateAsync<ProductDetails>(updateContext);

            details = DataContext.GetDetail<ProductDetails>(productId, 1);
            result = await stateManager.RetrieveDetails<ProductDetails>(details);
        }
    }
}