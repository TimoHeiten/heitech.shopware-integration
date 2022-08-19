namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

public class FilterFromAnonymous : IFilter
{
    private readonly object _anonymous;
    public FilterFromAnonymous(object anonymous)
        => _anonymous = anonymous;

    public object AsSearchInstance() => _anonymous;
}