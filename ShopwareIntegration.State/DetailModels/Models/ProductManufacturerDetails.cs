using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Manufacturer of a given Product
    /// <para/>
    /// Ressource can be found here https://shopware.stoplight.io/docs/admin-api/c2NoOjE0MzUxMjky-product-manufacturer
    ///</summary>
    [ModelUri("product-manufacturer")]
    public class ProductManufacturerDetails : DetailsEntity
    {
        public ProductManufacturerDetails()
        { }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; } = default!;

        [JsonPropertyName("customFields")]
        public object CustomFields { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("link")]
        public string Link { get; set; } = default!;

        [JsonPropertyName("media")]
        public object Media { get; set; } = default!;

        [JsonPropertyName("mediaId")]
        public string MediaId { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("products")]
        public object Products { get; set; } = default!;

        [JsonPropertyName("translated")]
        public object Translated { get; set; } = default!;

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = default!;

        [JsonPropertyName("versionId")]
        public string VersionId { get; set; } = default!;

    }
}