using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.DetailModels;

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

        public static (bool, object?) SearchExists<T>(this DataContext context) where T : DetailsEntity
        {
            bool exists = context.AdditionalData.TryGetValue(SEARCH, out object? search);

            return (exists, search);
        }


        public static (bool, string?) QueryExists<T>(this DataContext context) where T : DetailsEntity
        {
            bool exists = context.AdditionalData.TryGetValue(QUERY, out var query);

            return (exists, $"{query}");
        }

        public static void SetFilter(this DataContext context, object filter)
        {
            context.AdditionalData ??= new Dictionary<string, object>();
            context.AdditionalData.Add(FILTER, filter.FromAnonymous());
        }

        public static IFilter GetFilter(this DataContext context)
        {
            bool exists = context.AdditionalData.TryGetValue(FILTER, out var page);
            return exists ? (IFilter)page! : new object().FromAnonymous();
        }
    }
}