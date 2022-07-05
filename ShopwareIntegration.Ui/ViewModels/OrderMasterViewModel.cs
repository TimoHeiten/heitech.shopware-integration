using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class OrderMasterViewModel
{
    public string Id { get; }
    public float AmountTotal { get; set; }
    public string OrderNumber { get; }
    public DataContext DataContext { get; }
    
    public OrderMasterViewModel(OrderDetails detailsEntity, DataContext dataContext)
        => (Id, AmountTotal, OrderNumber, DataContext)
            = (detailsEntity.Id, detailsEntity.AmountTotal, detailsEntity!.OrderNumber!, dataContext);
}