
namespace ShopwareIntegration.Models.Filters
{
    public class FilterExpression : IParameter
    {
        public string Value { get; }
        public string Property { get; }
        public string? Expression { get; private set; }
        public string? Operator { get; private set; }
        public FilterExpression(string property, string value, string? expression, string? @operator)
            => (Property, Value, Expression, Operator) = (property, value, expression, @operator);

        public object ToAnonymousFilterObject()
        {
            if (Expression is not null)
                return new { Property, Value, Expression };

            if (Operator is not null)
                return new { Property, Value, Operator };

            return new { Property, Value };
        }
    }
}
