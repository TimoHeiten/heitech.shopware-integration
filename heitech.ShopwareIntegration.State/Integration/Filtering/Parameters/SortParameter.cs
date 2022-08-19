using System.Linq.Expressions;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.Integration.Models;

namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

public sealed class SortParameter<T> : IParameter
    where T : BaseEntity
{
    private string Field { get; }
    private string Order { get; }
    private bool NaturalSorting { get; }

    public SortParameter(Expression<Func<T, object>> propertyExpression, string order = FilterConstants.ASC, bool naturalSorting = false)
    {
        Order = order;
        NaturalSorting = naturalSorting;
        Field = propertyExpression.GetName<T>();
    }

    public object ToInstance()
    {
        return new {
            field = Field,
            order = Order,
            naturalSorting = NaturalSorting
        };
    }
}