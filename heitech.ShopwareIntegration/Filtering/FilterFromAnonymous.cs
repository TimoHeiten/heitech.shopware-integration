using heitech.ShopwareIntegration.Filtering;

namespace heitech.ShopwareIntegration.Filtering
{
    public class FilterFromAnonymous : IFilter
    {
        private readonly object _anonymous;
        public FilterFromAnonymous(object anonymous)
            => _anonymous = anonymous;

        public object AsSearchInstance() => _anonymous;
    }
}