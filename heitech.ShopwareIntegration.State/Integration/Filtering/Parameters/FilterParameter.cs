using System.Linq.Expressions;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.State.Integration.Models;


namespace heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;

public sealed class FilterParameter<T> : IParameter
    where T : BaseEntity
{
    public string Field { get; }
    public string Type { get; }
    public string Value { get; }
    public string[] Parameters { get; } = default!;

    public FilterParameter(Expression<Func<T, object>> fieldExpression, string type, string value)
    {
        Type = type;
        Value = value;
        Field = FilterConstants.GetName<T>(fieldExpression);
    }

    public FilterParameter(Expression<Func<T, object>> fieldExpression, string type, string[] parameters)
    {
        Type = type;
        Value = null!;
        Parameters = parameters;
        Field = FilterConstants.GetName<T>(fieldExpression);
    }

    public object ToInstance()
    {
        if (Parameters is null)
        {
            return new {
                field = Field,
                type = Type,
                parameters = Parameters
            };
        }
        else 
        {
            return new {
                field = Field,
                type = Type,
                value = Value
            };
        }
    }
}