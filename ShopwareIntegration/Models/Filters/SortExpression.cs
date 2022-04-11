namespace ShopwareIntegration.Models.Filters
{
    public class SortExpression : IParameter
    {
        public string Property { get; }
        public string? Direction { get; }

        public SortExpression(string property)
            => Property = property;

        public SortExpression(string property, string? direction)
            : this(property)
            => Direction = direction;

        public object ToAnonymousFilterObject()
        {
            if (Direction is not null)
                return new { Property, Direction };

            return new { Property };
        }
    }
}
