using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopwareIntegration.Models.Filters
{
    ///<summary>
    /// Paremetrize Get Requests with the FilterObject Array and its FilterExpressions
    ///</summary>
    public class FilterObject
    {
        public static FilterObject Empty => new FilterObject();
        public IEnumerable<object> Filters { get; set; } = Array.Empty<object>();
        
        // serialization / deserialization
        public FilterObject()
        { }

        public FilterObject(FilterBuilder builder)
            => Filters = builder.BuildFilter();

        public override string ToString()
            => string.Join(Environment.NewLine, Filters?.Select(x => $"{x}"));

        /*
        example from the api documentation
        https://developers.shopware.com/developers-guide/rest-api/
        GET /api/articles
        {
            "filter": [
                {
                    "property": "pseudoSales",
                    "expression": ">=",
                    "value": 1
                },
                {
                    "property": "active",
                    "value": 1
                }
            ]
        }
        */
    }
}
