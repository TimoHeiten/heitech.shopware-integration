using System.Collections.Generic;

namespace ShopwareIntegration.Models.Data
{
    public class DataArray<T>
    {
        [System.Text.Json.Serialization.JsonPropertyName("data")]
        public IReadOnlyList<T> Data { get; set; } = default!;
    }
}