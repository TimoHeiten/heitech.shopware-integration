namespace heitech.ShopwareIntegration.State.Integration.Models.Data;

public class DataArray<T>
{
    [System.Text.Json.Serialization.JsonPropertyName("data")]
    public IReadOnlyList<T> Data { get; set; } = default!;

    internal static DataArray<T> BuildForInsert(T first, params T[] more)
        => new DataArray<T>() { Data = new [] { first }.Concat(more).ToList() };
}