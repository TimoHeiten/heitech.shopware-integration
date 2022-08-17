using heitech.ShopwareIntegration.State.DetailModels;
namespace  heitech.ShopwareIntegration.State.StateManagerUtilities;

/// <summary>
/// Describes the Result of a multiple pages Query
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PageResult<T> where T : DetailsEntity
{
    public PageResult(int pageNo, IEnumerable<T> result, DataContext initialContext)
    {
        PageNo = pageNo;
        Result = result;
        InitialContext = initialContext;
    }

    public int PageNo { get; }
    public IEnumerable<T> Result { get; }
    public DataContext InitialContext { get; }
}
