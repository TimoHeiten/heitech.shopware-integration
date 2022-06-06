using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.Filtering
{
    ///<summary>
    /// Builder Object that allows to create Filters
    /// Not threadSafe!
    ///</summary>
    internal class FilterBuilder<T> : IFilterBuilder<T>, IFilter
        where T : BaseEntity
    {
        private List<SortParameter<T>> sortparameters = default!;
        private List<string> groupings = default!;
        private int? page = default!;
        private int? limit = default!;

        public FilterBuilder()
        { }

        public IFilterBuilder<T> Aggregate(AggregateParameter parameter, params AggregateParameter[] other)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Association<TOut>(Expression<Func<T, TOut>> propertyExpression)
        {
            throw new NotImplementedException();
        }

        public IFilterBuilder<T> Grouping(Expression<Func<T, object>> propExpression, params Expression<Func<T, object>>[] other)
        {
            this.groupings ??= new();
            this.groupings.AddRange(other.Concat(new [] { propExpression }).Select(x => FilterConstants.GetName<T>(x)));

            return this;
        }

        public IFilterBuilder<T> Limit(int limit)
        {
            this.limit ??= limit;
            return this;
        }

        public IFilterBuilder<T> Page(int pageNo)
        {
            this.page ??= pageNo;
            return this;
        }

        public IFilterBuilder<T> Sort(SortParameter<T> parameter, params SortParameter<T>[] other)
        {
            sortparameters ??= new();
            sortparameters.AddRange(other.Concat(new[] { parameter }));
            return this;
        }

        public IFilter Build() => this;

        public IFilter BuildAsPostFilter()
        {
            throw new NotImplementedException();
        }

        public object AsSearchInstance()
        {
            var sortArray = sortparameters?.Select(x => new { field = x.Field, order = x.Order, naturalSorting = x.NaturalSorting });
            return new {
                filter = new {
                    sort = sortArray?.ToArray(),
                    page = this.page,
                    limit = this.limit,
                    grouping = this.groupings?.ToArray()
                }
            };
        }
    }
}