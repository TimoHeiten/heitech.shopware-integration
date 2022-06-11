using System.Collections.Generic;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.Models
{
    [ModelUri("product")]
    public sealed class Product : BaseEntity
    {
        public static Product CreateNew()
            => new Product { Id = BaseEntity.CreateId };
        public Product()
        { }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

       [JsonPropertyName("availableStock")]
       public long AvailableStock { get; set; }

       [JsonPropertyName("description")]
       public string? Description { get; set; } = default!;

       [JsonPropertyName("ean")]
       public string? Ean { get; set; } = default!;

       [JsonPropertyName("stock")]
       public long Stock { get; set; }

       [JsonPropertyName("manufacturer")]
       public ProductManufacturer? Manufacturer { get; set; } = default!;

       [JsonPropertyName("manufacturerId")]
       public string? ManufacturerId { get; set; } = default!;

       [JsonPropertyName("price")]
       public IReadOnlyList<ProductPrice> Price { get; set; } = default!;
    }
}