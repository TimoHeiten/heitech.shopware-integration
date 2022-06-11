using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.Models
{
    [ModelUri("customer-group")]
    public sealed class CustomerGroup : BaseEntity
    {
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("customers")]
        public List<Customer> Customers { get; set; } = new();

        [JsonPropertyName("displayGross")]
        public bool DisplayGross { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("registrationActive")]
        public bool RegistrationActive { get; set; }

        public CustomerGroup() : base()
        { }

        public override string ToString()
        {
            return $"{CreatedAt} - {Customers?.Count} - {DisplayGross} - {Id} - {Name}";
        }
    }
}