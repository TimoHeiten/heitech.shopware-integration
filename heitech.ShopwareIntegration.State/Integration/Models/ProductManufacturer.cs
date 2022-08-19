using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.State.Integration.Configuration;

namespace heitech.ShopwareIntegration.State.Integration.Models;

[ModelUri("product-manufacturer")]
public class ProductManufacturer : BaseEntity
{
    public ProductManufacturer()
    { }

    [JsonPropertyName("name")]
    public string? Name { get; set; } = default!;
}