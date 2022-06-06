using System;
using System.Linq;
using System.Linq.Expressions;
using ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.Requests
{
    // todo...for now use the 
    ///<summary>
    /// Not working yet. For now use anonymous objects.
    ///</summary>
    public class FilterBuilder<T> : IFilterBuilder<T>, IFilter
        where T : BaseEntity
    {
        public FilterBuilder()
        { }

        public object AsSearchInstance()
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Aggregate(AggregateParameter parameter, params AggregateParameter[] other)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Association<TOut>(Expression<Func<T, TOut>> propertyExpression)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Grouping(string group, params string[] other)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Limit(int limit)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Page(int pageNo)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Sort(SortParameter parameter, params SortParameter[] other)
        {
            throw new NotImplementedException();
        }

        public IFilter Build()
        {
            throw new NotImplementedException();
        }

        public IFilter BuildAsPostFilter()
        {
            throw new NotImplementedException();
        }
    }

    public interface IFilterBuilder<T>
        where T : BaseEntity
    {
        const string ASC = "asc";
        IFilterBuilder<T> Page(int pageNo);
        IFilterBuilder<T> Limit(int limit);
        IFilterBuilder<T> Grouping(string group, params string[] other);
        IFilterBuilder<T> Sort(SortParameter parameter, params SortParameter[] other);
        IFilterBuilder<T> Association<TOut>(Expression<Func<T, TOut>> propertyExpression);
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

    public record SortParameter(string field, string Order = IFilterBuilder<CustomerGroup>.ASC, bool NaturalSorting = false);
    public record AggregateParameter(string Name, string Type, string field);

    public interface IFilter
    {
        object AsSearchInstance();
    }
}