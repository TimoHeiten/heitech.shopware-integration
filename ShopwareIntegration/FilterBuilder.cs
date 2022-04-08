using System.Collections.Generic;
using System.Linq;

namespace ShopwareIntegration.Models.Filters
{
    public class FilterBuilder
    {
        public static FilterBuilder Empty = new();
        private List<FilterExpression> _filter = new(capacity: 4);
        private List<SortExpression> _sort = new(capacity: 4);
        private LimitExpression? _limit;

        public FilterBuilder AddFilter(string property, string value, string? expression = null, string? @operator = null)
        {
            _filter.Add(new FilterExpression(property, value, expression, @operator));
            return this;
        }

        public FilterBuilder AddSort(string property, string? direction = null)
        {
            _sort.Add(new SortExpression(property, direction));
            return this;
        }

        public FilterBuilder AddLimit(int limit, int? start = null)
        {
            if (_limit is not null)
                return this;

            _limit ??= new LimitExpression(limit, start);
            return this;
        }

        public IEnumerable<object> BuildFilter()
        {
            if (_sort.Any())
                yield return new { Sort = _sort.Select(x => x.ToAnonymousFilterObject()).ToArray() };

            if (_filter.Any())
                yield return new { Filter = _filter.Select(x => x.ToAnonymousFilterObject()).ToArray() };

            if (_limit is not null)
                yield return _limit.ToAnonymousFilterObject();
        }
    }
}
