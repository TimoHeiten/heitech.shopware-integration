using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Api
{
    public static class ApiParameter
    {
        public const string QUERY = "query";

        public const string SEARCH = "search";

        public static (bool, object?) SearchExists<T>(this DataContext ctxt) where T : DetailsEntity
        {
            if (ctxt.AdditionalData is null) return (false, default!);

            bool exists = ctxt.AdditionalData.TryGetValue(SEARCH, out object? search);

            return (exists, search);
        }


        public static (bool, string?) QueryExists<T>(this DataContext ctxt) where T : DetailsEntity
        {
            if (ctxt.AdditionalData is null) return (false, default!);

            bool exists = ctxt.AdditionalData.TryGetValue(QUERY, out object? query);

            return (exists, $"{query}");
        }
    }
}