using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class ProductMasterViewModel
{
    public string Id { get; }
    public string  Ean { get; }
    public string Description { get; }
    public string ManufacturerId { get; }
    public DataContext DataContext { get; }
    
    public ProductMasterViewModel(ProductDetails detailsEntity, DataContext dataContext)
        => (Id, Description, Ean, ManufacturerId, DataContext)
            = (detailsEntity.Id, detailsEntity!.Description!, detailsEntity.Ean, detailsEntity.ManufacturerId, dataContext);

}