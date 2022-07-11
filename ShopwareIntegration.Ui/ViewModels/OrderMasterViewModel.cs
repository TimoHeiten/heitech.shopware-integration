using System;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class OrderMasterViewModel : DetailViewModelBase
{
    public float AmountTotal { get; }
    public DateTime? OrderDate { get; }
    public string OrderNumber { get; }

    public OrderMasterViewModel(OrderDetails detailsEntity, DataContext dataContext)
        => (Id, AmountTotal, OrderDate, OrderNumber, Context)
            = (detailsEntity.Id, detailsEntity.AmountTotal, detailsEntity!.OrderDateTime!, detailsEntity.OrderNumber, dataContext);

    public override string Type => DetailTypes.ORDERS;
}