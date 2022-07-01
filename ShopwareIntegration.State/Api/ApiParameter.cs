using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Api
{
    public static class ApiParameter
    {
        public const string QUERY = "query";
        public const string PAGING = "paging";
        public const string FILTER = "filter";
        public const string SEARCH = "search";

        public static (bool, object?) SearchExists<T>(this DataContext ctxt) where T : DetailsEntity
        {
            bool exists = ctxt.AdditionalData.TryGetValue(SEARCH, out object? search);

            return (exists, search);
        }


        public static (bool, string?) QueryExists<T>(this DataContext ctxt) where T : DetailsEntity
        {
            bool exists = ctxt.AdditionalData.TryGetValue(QUERY, out object? query);

            return (exists, $"{query}");
        }

        public static void SetFilter(this DataContext ctxt, object filter)
        {
            ctxt.AdditionalData ??= new Dictionary<string, object>();
            ctxt.AdditionalData.Add(FILTER, filter.FromAnonymous());
        }

        public static IFilter GetFilter(this DataContext ctxt)
        {
            bool exists = ctxt.AdditionalData.TryGetValue(FILTER, out var page);
            return exists ? (IFilter)page! : new object().FromAnonymous();
        }
    }
}