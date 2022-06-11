namespace heitech.ShopwareIntegration.Filtering.Parameters
{

    public interface IParameter { object ToInstance(); }

    public sealed record AggregateParameter(string Name, string Type, string field);
}