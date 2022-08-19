namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

public sealed class EmptyFilter : IFilter
{
    private EmptyFilter() { }
    public static IFilter Instance { get; } = new EmptyFilter();
    public object AsSearchInstance() => null!; // for serialization as empty object
}