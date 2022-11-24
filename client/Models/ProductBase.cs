using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using Newtonsoft.Json;

namespace client.Models;

[ModelUri("product")]
public sealed class ProductBase : IHasShopwareId
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; } = default!;
}