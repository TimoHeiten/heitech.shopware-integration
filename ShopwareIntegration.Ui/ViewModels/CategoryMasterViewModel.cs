using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class CategoryMasterViewModel
{
    public string Id { get; }
    public string Description { get; }
    public DataContext DataContext { get; }
    
    public CategoryMasterViewModel(CategoryDetails detailsEntity, DataContext dataContext)
        => (Id, Description, DataContext)
            = (detailsEntity.Id, detailsEntity!.Description!, dataContext);
}