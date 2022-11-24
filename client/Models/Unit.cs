using System;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using Newtonsoft.Json;

namespace client.Models;

[ModelUri("unit")]
public sealed class Unit : IHasShopwareId
{
    [JsonProperty("id")] public string? Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonProperty("name")] public string? Name { get; set; } = "name";

    [JsonProperty("shortCode")] public string? ShortCode { get; set; } = "name-code";

    [JsonProperty("translated")] public object? Translated { get; }

    // should be datetime -> figure out format
    [JsonProperty("createdAt")] public string? CreatedAt { get; set; } = "2022-06-04T23:56:05.804128+02:00";

    [JsonProperty("updatedAt")] public string? UpdatedAt { get; set; } = "2022-06-04T23:56:05.819296+02:00";

    [JsonProperty("products")] public object? Products { get; set; } = Array.Empty<object>();
}