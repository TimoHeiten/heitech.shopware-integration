using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

namespace heitech.ShopwareIntegration.State.Api
{
    ///<summary>
    /// Extension methods to add parameters for the Shopware ApiRequests (add to additionalData Map)
    ///</summary>
    public static class ApiParameter
    {
        private const string QUERY = "query";
        private const string PAGING = "paging";
        private const string FILTER = "filter";
        private const string SEARCH = "search";

        internal static (bool, object?) SearchExists<T>(this DataContext context) where T : DetailsEntity
        {
            object? search = default!;
            bool? exists = context.AdditionalData?.TryGetValue(SEARCH, out search);

            return (exists.HasValue && exists.Value, search);
        }


        internal static (bool, string?) QueryExists<T>(this DataContext context) where T : DetailsEntity
        {
            object? query = default!;
            var exists = context.AdditionalData?.TryGetValue(QUERY, out query);

            return (exists.HasValue && exists.Value, $"{query}");
        }

        public static void SetFilter(this DataContext context, object filter)
        {
            context.AdditionalData ??= new Dictionary<string, object>();
            context.AdditionalData.Add(FILTER, filter.FromAnonymous());
        }

        internal static IFilter GetFilter(this DataContext context)
        {
            object? page = default!;
            bool? exists = context.AdditionalData?.TryGetValue(FILTER, out page);
            
            return exists.HasValue && exists.Value ? (IFilter)page! : new object().FromAnonymous();
        }
    }
}