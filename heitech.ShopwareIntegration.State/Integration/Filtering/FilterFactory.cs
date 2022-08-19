using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.Integration.Models;

namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

public static class FilterFactory
{
    public static IFilterBuilder<T> CreateBuilder<T>() where T : BaseEntity
        => new FilterBuilder<T>();

    ///<summary>
    /// Supply an anonymous object as the filter specification.
    ///</summary>
    public static IFilter FromAnonymous(this object obj) => new FilterFromAnonymous(obj);
}