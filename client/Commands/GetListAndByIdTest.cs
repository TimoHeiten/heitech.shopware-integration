using System;
using System.Linq;
using System.Threading.Tasks;
using client.Models;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using heitech.ShopwareIntegration.Core;
using Newtonsoft.Json;

namespace client.Commands;

[Command("getList", Description = "GetList of Products with page=1 & limit=10")]
public sealed class GetProductListWithFilter : ICommand
{
    private readonly IShopwareClient _client;

    public GetProductListWithFilter(IShopwareClient client)
        => _client = client;
    
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var filter = _client.CreateFilterFromAnonymous(new { page=1, limit=10});
        await console.Output.WriteLineAsync($"Get with filter [{JsonConvert.SerializeObject(filter.Value)}]");

        var result = await _client.SearchAsync<ProductBase>(filter);
        if (result.IsSuccess)
        {
            var ids = string.Join(Environment.NewLine, result.Model.Data.Select(x => x.Id));
            await console.Output.WriteLineAsync($"Success: see ids -> [{ids}]");
            return;
        }
        
        await console.Output.WriteLineAsync($"Failed: {result.Exception.Message}");
    }
}