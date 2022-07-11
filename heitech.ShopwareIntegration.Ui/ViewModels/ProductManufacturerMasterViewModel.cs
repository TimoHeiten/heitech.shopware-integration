using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class ProductManufacturerMasterViewModel : DetailViewModelBase
{
    public string Description { get; }
    
    public ProductManufacturerMasterViewModel(ProductManufacturerDetails detailsEntity, DataContext dataContext)
        => (Id, Description, Context)
            = (detailsEntity.Id, detailsEntity!.Description!, dataContext);
    
    public override string Type => DetailTypes.MANUFACTURERS;
}