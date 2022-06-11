using System;
using System.Linq;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.Models
{
    [ModelUri("product-manufacturer")]
    public class ProductManufacturer : BaseEntity
    {
        public ProductManufacturer()
        { }

        [JsonPropertyName("name")]
        public string? Name { get; set; } = default!;
    }
}