using System.Linq.Expressions;
using heitech.ShopwareIntegration.State.Integration.Models;

namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

public interface IFilterBuilder<T>
    where T : BaseEntity
{
    IFilterBuilder<T> Page(int pageNo);
    IFilterBuilder<T> Limit(int limit);
    IFilterBuilder<T> Grouping(Expression<Func<T, object>> propExpression, params Expression<Func<T, object>>[] other);
    IFilterBuilder<T> Sort(SortParameter<T> parameter, params SortParameter<T>[] other);
    IFilterBuilder<T> Association<TOut>(Expression<Func<T, TOut>> propertyExpression, IFilter nestedFilter = null!);
    IFilterBuilder<T> Aggregate(AggregateParameter parameter, params AggregateParameter[] other);
        
    ///<summary>
    /// Build the Filter to be used in your specified api/search/* POST endpoint
    ///</summary>
    IFilter Build();
    ///<summary>
    /// Keeps the aggregations in place
    ///</summary>
    IFilter BuildAsPostFilter();
}