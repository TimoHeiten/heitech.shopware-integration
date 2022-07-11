using System.Windows.Controls;
using heitech.ShopwareIntegration.State;
using ShopwareIntegration.Ui.Components.Forms;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class ProductDetailViewModel : DetailViewModelBase
{
    public ProductMasterViewModel Product { get; }
    public ProductManufacturerMasterViewModel Manufacturer { get; }
    
    public ProductDetailViewModel(ProductMasterViewModel product, ProductManufacturerMasterViewModel manufacturer, DataContext context)
    {
        Id = product.Id;
        Context = context;
        Product = product;
        Manufacturer = manufacturer;
    }

    public override string Type => DetailTypes.PRODUCTS;

    public override Control GenerateViewData()
    {
        return new ProductForm(this);
    }
}