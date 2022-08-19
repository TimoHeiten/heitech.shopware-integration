using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.StateManagerUtilities;

namespace client;

public class Benchmarked
{
    private IStateManager _stateManager;
    [GlobalSetup]
    public async Task SetupStateManager()
    {
        var configuration = new HttpClientConfiguration(
            baseUrl: "http://sw6.wbv24.com/api/",
            clientId: "SWIATKTYADFGUWC2CM53VFKWBG",
            userName: string.Empty,
            clientSecret: "Nk9XQWQzSkRwVnQ2T01LTzJydnM5M3RQTFVJNW1SY3NJM3NTckY"
        );
        _stateManager = await Program.WithOutCache(configuration);
    }
    
    [Benchmark]
    public async Task MultiplePagesSequentially()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < 3; i++)
        {
            var context = DataContext.GetPage<ProductDetails>(i+1);
            context.SetFilter(Program.ProductFilter(i+1));
            _ = await _stateManager.RetrievePage<ProductDetails>(context);
            // do nothing with result, just for (lazily written perf benchmark)
        }
        stopWatch.Stop();
        Console.WriteLine($"sequential took {stopWatch.ElapsedMilliseconds} ms");
    }
    [Benchmark]
    public async Task MultiplePagesConcurrently()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var descr = new PageQueryDescription[]
        {
            new(1, Program.ProductFilter(1)),
            new(2, Program.ProductFilter(2)),
            new(3, Program.ProductFilter(3))
        };
        await _stateManager.GetMultiplePagesConcurrently<ProductDetails>(descr);
        stopWatch.Stop();
        Console.WriteLine($"parallel took {stopWatch.ElapsedMilliseconds} ms");
    }
}