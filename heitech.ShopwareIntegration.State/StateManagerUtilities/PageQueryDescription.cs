namespace heitech.ShopwareIntegration.State.StateManagerUtilities;

/// <summary>
/// Describe a single Page Request for a multiple pages query.
/// </summary>
public sealed class PageQueryDescription
{
    public PageQueryDescription(ushort pageNo, object filter, Dictionary<string, object>? additionalData = null!)
    {
        PageNo = pageNo;
        Filter = filter;
        AdditionalData = additionalData;
    }

    public ushort PageNo { get; }
    public object Filter { get; }
    public Dictionary<string, object>? AdditionalData { get; }
    
}