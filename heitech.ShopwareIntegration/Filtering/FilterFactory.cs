using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.Filtering
{
    public static class FilterFactory
    {
        public static IFilterBuilder<T> CreateBuilder<T>() where T : BaseEntity
            => new FilterBuilder<T>();

        ///<summary>
        /// Supply an anonymous object as the filter specification.
        ///</summary>
        public static IFilter FromAnonymous(this object obj) => new FilterFromAnonymous(obj);
    }
}