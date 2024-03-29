using System.Text.Json.Serialization;

namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

/// <summary>
/// specific filter to include the desired fields 
/// </summary>
public class IncludesFields
{
    public sealed class Product : IncludesFields
    {
        [JsonPropertyName("product")]
        public string[] FieldsProducts { get; }
        public Product(params string[] fields)
            => (FieldsProducts) = (fields);
    }
    
    public sealed class Category : IncludesFields
    {
        [JsonPropertyName("category")]
        public string[] FieldsCategories { get; }
        public Category(params string[] fields)
            => (FieldsCategories) = (fields);
    }
        
    public sealed class Order : IncludesFields
    {
        [JsonPropertyName("order")]
        public string[] FieldsOrder { get; }
        public Order(params string[] fields)
            => (FieldsOrder) = (fields);
    }
        
    public sealed class ProductManufacturer : IncludesFields
    {
        [JsonPropertyName("productManufacturer")]
        public string[] FieldsProductManufacturer { get; }
        public ProductManufacturer(params string[] fields)
            => (FieldsProductManufacturer) = (fields);
    }
}