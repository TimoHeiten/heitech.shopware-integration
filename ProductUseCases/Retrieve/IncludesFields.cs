using System.Text.Json.Serialization;

namespace heitech.ShopwareIntegration.ProductUseCases
{
    public sealed class IncludesFields
    {
        [JsonPropertyName("product")]
        public string[] Fields_Products { get; }
        public IncludesFields(params string[] fields)
            => (Fields_Products) = (fields);
    }
}
