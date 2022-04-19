using System.Collections.Generic;
using System.Linq;
using ShopwareIntegration.Filter;

namespace ShopwareIntegration
{
    ///<summary>
    /// Not yet finished: see here for more details https://shopware.stoplight.io/docs/store-api/ZG9jOjEwODExNzU2-search-queries
    ///</summary>
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

        ///<summary>
        /// CAUTION: Does not support the full range of available Shopware Filters as of yet. Instead use a simple Dictionary<strin, object> for associations and alike (see https://shopware.stoplight.io/docs/store-api/ZG9jOjEwODExNzU2-search-queries for more info)
        ///</summary>
        public IEnumerable<object> BuildFilter()
        {
            if (_sort.Any())
                yield return new { Sort = _sort.Select(x => x.AsContent()).ToArray() };

            if (_filter.Any())
                yield return new { Filter = _filter.Select(x => x.AsContent()).ToArray() };

            if (_limit is not null)
                yield return _limit.AsContent();
        }
    }
}
