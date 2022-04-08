using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace models.filters
{
    public class FilterObject
    {
        public IEnumerable<FilterExpression> Filter { get; set; } = default!;

        public override string ToString()
            => string.Join(", ", Filter is null ? "" : Filter.Select(x => $"{x.Property} - {x.Expression} - {x.Value}"));

        public static FilterObject Empty = new() { Filter = Enumerable.Empty<FilterExpression>() };
    }

    public class FilterExpression
    {
        public string Property { get; }

        public string Expression { get; }
        public string Value { get; }

        [JsonConstructor]
        public FilterExpression(string property, string expression, string value)
            => (Property, Expression, Value) = (property, expression, value);
    }
}
