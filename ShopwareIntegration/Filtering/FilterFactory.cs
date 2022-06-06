using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.Models;
using ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.Filtering
{
    public class FilterFactory
    {
        public static IFilterBuilder<T> CreateBuilder<T>() where T : BaseEntity
            => new FilterBuilder<T>();
    }
}