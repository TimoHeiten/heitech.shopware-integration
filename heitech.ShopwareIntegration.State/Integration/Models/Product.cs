using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.State.Integration.Configuration;

namespace heitech.ShopwareIntegration.State.Integration.Models;

[ModelUri("product")]
public sealed class Product : BaseEntity
{
    ///<summary>
    /// Use just for the Serialization. For other caeses just use the static Factory methods instead.
    ///</summary>
    public Product()
    { }

    [JsonPropertyName("active")]
    public bool? Active { get; set; } = null!;

    [JsonPropertyName("availableStock")]
    public long? AvailableStock { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; } = default!;

    [JsonPropertyName("ean")]
    public string? Ean { get; set; } = default!;

    [JsonPropertyName("manufacturer")]
    public ProductManufacturer? Manufacturer { get; set; } = default!;

    [JsonPropertyName("manufacturerId")]
    public string? ManufacturerId { get; set; } = default!;


    #region Required fields for Insert

    ///<summary>
    /// Does not return a Product but an anonymous object instead, since the API is very peculiar and only accepts the correct amount of properties
    /// feel free to add an explicit (typesafe) Model for write operations yourself
    ///</summary>
    public static object NewProduct(string name, ProductPrice price, string productNumber, int initialStock, string taxId)
    {
        return new
        {
            name = name,
            taxId = taxId,
            stock = initialStock,
            id = BaseEntity.CreateId,
            createdAt = DateTime.UtcNow,
            productNumber = productNumber,
            price = new [] { 
                new {
                    id = price.Id,
                    net = price.Net,
                    gross = price.Gross,
                    linked = price.Linked,
                    currencyId = price.CurrencyId,
                }
            },
            ean = "deine ean"
        };
    }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; } = default!;

    [JsonPropertyName("price")]
    public IReadOnlyList<ProductPrice> Price { get; set; } = default!;

    [JsonPropertyName("productNumber")]
    public string? ProductNumber { get; set; } = default!;

    [JsonPropertyName("stock")]
    public long Stock { get; set; }

    [JsonPropertyName("taxId")]
    public string TaxId { get; set; } = default!;
    #endregion
}