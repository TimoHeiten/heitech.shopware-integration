namespace heitech.ShopwareIntegration.Core.Filters
{
    /// <summary>
    /// Marker interface for Shopware filter, usable in Search Body requests and others
    /// </summary>
    public interface IFilter
    {
        object Value { get; }
    }
}