using System;
using System.Linq.Expressions;
using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.Filtering
{
    public sealed class SortParameter<T>
        where T : BaseEntity
    {
        public string Field { get; }
        public string Order { get; }
        public bool NaturalSorting { get; }

        public SortParameter(Expression<Func<T, object>> propertyExpression, string order = FilterConstants.ASC, bool naturalSorting = false)
        {
            NaturalSorting = naturalSorting;
            Order = order;
            Field = propertyExpression.GetName<T>();
        }
    }

    public sealed record AggregateParameter(string Name, string Type, string field);
}