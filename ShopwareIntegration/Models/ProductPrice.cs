using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.Models
{
    // not a Model in the sense that it is its own entity/resource.
    // instead it is just a part of the Product Ressource
    public sealed class ProductPrice : BaseEntity
    {
        public ProductPrice()
        { }

        [JsonPropertyName("net")]
        public decimal Net { get; set; }

        [JsonPropertyName("gross")]
        public decimal Gross { get; set; }

        [JsonPropertyName("currencyId")]
        public string? CurrencyId { get; set; } = default!;
    }
}