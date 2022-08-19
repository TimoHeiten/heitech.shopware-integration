namespace heitech.ShopwareIntegration.State.Integration.Models.Data;

public class DataObject<T>
{
    [System.Text.Json.Serialization.JsonPropertyName("data")]
    public T Data { get; set; } = default!;
}