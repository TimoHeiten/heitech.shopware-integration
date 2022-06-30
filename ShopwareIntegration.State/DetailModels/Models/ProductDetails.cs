using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// represents a DTO with all available properties of the Products Ressource
    ///</summary>
    public class ProductDetails : DetailsEntity
    {
        // ctor for serialization purposes
        public ProductDetails()
        { }

        [JsonPropertyName("price")]
        public IReadOnlyList<ProductPrice> Price { get; set; } = default!;

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("availableStock")]
        public long? AvailableStock { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; } = default!;
    }
}