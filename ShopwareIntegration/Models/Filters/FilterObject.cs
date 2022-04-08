using System.Collections.Generic;
using System.Linq;

namespace ShopwareIntegration.Models.Filters
{
    ///<summary>
    /// Paremetrize Get Requests with the FilterObject Array and its FilterExpressions
    ///</summary>
    public class FilterObject
    {
        // todo
        public static FilterObject Empty => new FilterObject();
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
