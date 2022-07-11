using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class CategoryMasterViewModel : DetailViewModelBase 
{
    public string Description { get; }
    
    public CategoryMasterViewModel(CategoryDetails detailsEntity, DataContext dataContext)
        => (Id, Description, Context)
            = (detailsEntity.Id, detailsEntity!.Description!, dataContext);

    public override string Type => DetailTypes.CATEGORIES;
}