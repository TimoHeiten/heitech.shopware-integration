using System.Threading.Tasks;
using client.Models;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using heitech.ShopwareIntegration.Core;

namespace client.Commands;

[Command("getById", Description = "Get Product by Id")]
public sealed class GetProductByIdCommand : ICommand
{
    [CommandOption("id", 'i', Description = "Id for the Product", IsRequired = true)]
    public string Id { get; set; } = default!;

    private readonly IShopwareClient _client;
    public GetProductByIdCommand(IShopwareClient client)
        => _client = client;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        await console.Output.WriteLineAsync($"Get with Id [{Id}]");

        var result = await _client.GetByIdAsync<ProductBase>(Id);
        if (result.IsSuccess)
        {
            await console.Output.WriteLineAsync($"Success: see ids -> [{result.Model.Data.Id} - {result.Model.Data.Description} - ]");
            return;
        }
        
        await console.Output.WriteLineAsync($"Failed {result.Exception.Message}");
        
    }
}