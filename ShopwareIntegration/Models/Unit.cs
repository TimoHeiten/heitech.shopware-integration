using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.Models
{
    [ModelUri("unit")]
    public sealed class Unit : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("shortCode")]
        public string ShortCode { get; set; } = default!;

        [JsonPropertyName("translated")]
        public bool Translated { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("customFields")]
        public object CustomFields { get; set; } = default!;

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();

        public Unit() : base()
        { }
    }
}