using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.StateManagerUtilities;
using ShopwareIntegration.Requests;

namespace heitech.ShopwareIntegration.State;

// encapsulates the DataContext
// makes parallel Requests for multiple pages
// returns an OperationResult instead of throwing exceptions 
public static class StateManagerExtensions
{
    public static async Task<RequestResult<T>> GetDetail<T>(this IStateManager stateManager, string id, int pageNo, Dictionary<string, object> additionalData = default!) 
        where T : DetailsEntity
    {
        var dataContext = DataContext.GetDetail<T>(id, pageNo, additionalData);
        try
        {
            var result = await stateManager.RetrieveDetails<T>(dataContext);
            return RequestResult<T>.Success(result);
        }
        catch (Exception e)
        {
            return RequestResult<T>.Failed(e); 
        }
    }

    /// <summary>
    /// Makes a parallel Request for better performance of aquiring multiple pages at once. use the includes field on the filter object
    /// to only get the required fields to further improve performance of the api! 
    /// </summary>
    /// <param name="stateManager"></param>
    /// <param name="pageQueryDescriptions"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<RequestResult<IEnumerable<PageResult<T>>>> GetMultiplePagesConcurrently<T>(this IStateManager stateManager, IEnumerable<PageQueryDescription> pageQueryDescriptions)
        where T : DetailsEntity
    {
        var ordered = pageQueryDescriptions.OrderBy(x => x.PageNo).ToArray();
        if (ordered.Any() is false)
            return RequestResult<IEnumerable<PageResult<T>>>.Failed(new Exception("no pages where requested"));

        var start = ordered[0].PageNo;
        var last = ordered[^1].PageNo;
        var range = new PageRange(start, last);
        if (!range.IsValid())
            return RequestResult<IEnumerable<PageResult<T>>>.Failed(new Exception($"Range for Pages was not valid {range.Start} - {range.End}"));

        var dataContexts = ordered.Select((page) => GetPageResult<T>(stateManager, page)).ToList();
        var results = await Task.WhenAll(dataContexts);

        return RequestResult<IEnumerable<PageResult<T>>>.Success(results);
    }

    private static async Task<PageResult<T>> GetPageResult<T>(IStateManager stateManager, PageQueryDescription descr) where T : DetailsEntity
    {
        var context = DataContext.GetPage<T>(descr.PageNo, descr.AdditionalData);
        context.SetFilter(descr.Filter);
        var pageItems = await stateManager.RetrievePage<T>(context);
        return new PageResult<T>(descr.PageNo, pageItems, context);
    }
}
