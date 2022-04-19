using System;

namespace ShopwareIntegration.Filter
{
    public class FilterExpression : IParameter
    {
        public string Value { get; }
        public string Property { get; }
        public string? Operator { get; private set; }
        public string? Expression { get; private set; }

        public FilterExpression(string property, string value, string? expression, string? @operator)
            => (Property, Value, Expression, Operator) = (property, value, expression, @operator);

        public object AsContent()
        {
            if (Expression is not null)
                return new { Property, Value, Expression };

            if (Operator is not null)
                return new { Property, Value, Operator };

            return new { Property, Value };
        }
    }
}
