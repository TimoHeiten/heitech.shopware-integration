namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters; 

public interface IParameter
{
    object ToInstance();
}

public sealed record AggregateParameter(string Name, string Type, string field);