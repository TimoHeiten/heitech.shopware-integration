using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;
using heitech.ShopwareIntegration.State.Interfaces;
using ShopwareIntegration.Ui.ViewModels;

namespace ShopwareIntegration.Ui.Services;

/// <summary>
/// StateService Singleton. If more services are required when the application grows, try to use a factory and per CreateInstance method instance or be careful
/// </summary>
public sealed class StateService
{
    private IStateManager? _state = null!;
    private Commands? _commands = null!;
    public Commands Commands => _commands ?? (_commands = new Commands(_state!));

    private readonly SemaphoreSlim _mutex = new(1);
    private static HttpClientConfiguration? _configuration = default!;

    public static StateService CreateInstance(HttpClientConfiguration configuration)
    {
        _configuration = configuration;
        return new StateService();
    }

    public void ForceNewInstance() => _state = null;

    private StateService()
    { }

    private async Task<IStateManager> InitializeAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            if (_state is null && _configuration is not null)
                _state = await Factory.CreateAsync(_configuration);
        }
        finally
        {
            _mutex.Release();
        }

        return _state!;
    }

    private async Task<IEnumerable<TViewModel>> GetAsync<T, TViewModel>(int pageNo,
        Func<T, DataContext, TViewModel> selector, object? includes = null)
        where T : DetailsEntity
    {
        var state = await InitializeAsync();
        var filter = new
        {
            limit = 100,
            page = pageNo,
            includes = includes
        };
        var pageContext = DataContext.GetPage<T>(filter.page);
        pageContext.SetFilter(filter);

        var details = (await state.RetrievePage<T>(pageContext))
            .Select(x => selector(x, pageContext))
            .ToArray();

        return details;
    }

    public Task<IEnumerable<ProductMasterViewModel>> GetProductsAsync(int page)
    {
        return GetAsync<ProductDetails, ProductMasterViewModel>(
            page,
            (detail, context) => new ProductMasterViewModel(detail, context),
            includes: new IncludesFields.Product(
                "name", "price", "id", "active", "availableStock", "description", "ean",
                "stock", "manufacturerId"
            )
        );
    }

    public Task<IEnumerable<CategoryMasterViewModel>> GetCategoriesAsync(int page)
    {
        return GetAsync<CategoryDetails, CategoryMasterViewModel>(
            page,
            (detail, context) => new CategoryMasterViewModel(detail, context),
            includes: new IncludesFields.Category("name", "description")
        );
    }

    public Task<IEnumerable<OrderMasterViewModel>> GetOrdersAsync(int page)
    {
        return GetAsync<OrderDetails, OrderMasterViewModel>(
            page,
            (detail, context) => new OrderMasterViewModel(detail, context),
            includes: new IncludesFields.Order("orderDate", "orderNumber", "amountTotal")
        );
    }

    public Task<IEnumerable<ProductManufacturerMasterViewModel>> GetProductManufacturersAsync(int page)
    {
        return GetAsync<ProductManufacturerDetails, ProductManufacturerMasterViewModel>(
            page,
            (detail, context) => new ProductManufacturerMasterViewModel(detail, context),
            includes: new IncludesFields.ProductManufacturer("name", "description")
        );
    }

    public async Task<ProductDetailViewModel> GetProductByIdAsync(RessourceId id, DataContext fromPage)
    {
        var context = DataContext.GetDetail<ProductDetails>(id, fromPage.PageNo);
        context.SetFilter(
            new
            {
                includes = new IncludesFields.Product("id", "name", "description", "ean", "manufacturerId")
            }.FromAnonymous()
        );

        var productDetails = await _state!.RetrieveDetails<ProductDetails>(context).ConfigureAwait(false);
        var manufacturerDetails = await _state!.RetrieveDetails<ProductManufacturerDetails>(
            DataContext.GetDetail<ProductManufacturerDetails>(productDetails.ManufacturerId, fromPage.PageNo)
        ).ConfigureAwait(false);

        return new ProductDetailViewModel(
            new ProductMasterViewModel(productDetails, context),
            new ProductManufacturerMasterViewModel(manufacturerDetails, context), 
            context
        );
    }
}