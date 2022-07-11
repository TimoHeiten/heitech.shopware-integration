using System;
using System.Threading.Tasks;
using System.Windows;
using heitech.ShopwareIntegration.Models;
using heitech.ShopwareIntegration.ProductUseCases;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace ShopwareIntegration.Ui.Services;

/// <summary>
/// Mutate Resources of the Shopware API
/// </summary>
public sealed class Commands
{
    private readonly IStateManager _stateManager;

    public Commands(IStateManager stateManager)
        => _stateManager = stateManager;

    private Task<ProductDetails> GetDetails(DataContext context)
    {
        context.SetFilter(new
        {
            includes = new IncludesFields.Product(new[] { "id", "stock", "availableStock", "price" })
        });
        return _stateManager.RetrieveDetails<ProductDetails>(context);
    }

    public async Task<bool> UpdateProduct(string id, int pageNo, ProductPrice? newPrice = null, int newStock = 0,
        int availableStock = 0)
    {
        var detailsContext = DataContext.GetDetail<ProductDetails>(id, pageNo);
        var details = await GetDetails(detailsContext);
        if (details is null)
            throw new Exception(
                $"Product with Id:'{id}' at page-{pageNo} could not be found");

        const int single = 0;
        var update = new
        {
            availableStock = details.AvailableStock + availableStock,
            stock = details.Stock + newStock,
            price = newPrice ?? ProductPrice.NewPrice(details.Price[single].Net, details.Price[single].Gross,
                details.Price[single].CurrencyId, details.Price[single].Linked!)
        };
        detailsContext.AddUpdate(new { active = true });

        try
        {
            _ = await _stateManager.UpdateAsync<ProductDetails>(detailsContext);
            return true;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
            return false;
        }
    }
}