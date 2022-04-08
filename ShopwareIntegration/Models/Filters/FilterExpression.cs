using System;
using System.Text.Json.Serialization;

namespace ShopwareIntegration.Models.Filters
{
    public class FilterExpression
    {
        public string Property { get; set; }
        public string Expression { get; set; }
        public string Value { get; set; }

        [JsonConstructor]
        public FilterExpression(string property, string expression, string value)
            => (Property, Expression, Value) = (property, expression, value);
    }

    public class SortExpression
    {
        public string Property { get; set; } = default!;
        public string Direction { get; set; } = "ASC";
    }

    public class LimitExpression
    {
        public int Limit { get; set; }
        public int Start { get; set; }
    }


}
