using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShopwareIntegration.Configuration;

namespace ShopwareIntegration.Models
{
    [ModelUri("customer-group")]
    public class CustomerGroup : BaseEntity
    {
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("customers")]
        public List<Customer> Customers { get; set; } = new();

        [JsonPropertyName("displayGross")]
        public bool DisplayGross { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("registrationActive")]
        public bool RegistrationActive { get; set; }

        public override string ToString()
        {
            return $"{CreatedAt} - {Customers?.Count} - {DisplayGross} - {Id} - {Name}";
        }
    }
}