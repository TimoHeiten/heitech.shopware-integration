using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class ProductMasterViewModel : DetailViewModelBase
{
    public string  Ean { get; }
    private string _description;
    public string Description => _description?[..150] ?? "";
    public string ManufacturerId { get; }
    
    public ProductMasterViewModel(ProductDetails detailsEntity, DataContext dataContext)
        => (Id, _description, Ean, ManufacturerId, Context)
            = (detailsEntity.Id, detailsEntity!.Description!, detailsEntity.Ean, detailsEntity.ManufacturerId, dataContext);
    public override string Type => DetailTypes.PRODUCTS;
}