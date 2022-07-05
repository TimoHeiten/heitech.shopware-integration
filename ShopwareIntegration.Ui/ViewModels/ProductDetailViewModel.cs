using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.ViewModels;

public sealed class ProductDetailViewModel
{
    public ProductDetails Product { get; }
    public ProductManufacturerDetails Manufacturer { get; }
    
    public ProductDetailViewModel(ProductDetails product, ProductManufacturerDetails manufacturer)
    {
        Product = product;
        Manufacturer = manufacturer;
    }
}