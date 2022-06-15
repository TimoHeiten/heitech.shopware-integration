using System.Text.Json.Serialization;

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

        [JsonPropertyName("linked")]
        public bool? Linked { get; set; } = false;
    }
}