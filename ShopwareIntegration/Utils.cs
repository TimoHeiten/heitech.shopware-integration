using System;
using ShopwareIntegration.Models.Filters;

namespace ShopwareIntegration
{
    public static class Utils
    {
        ///<summary>
        /// Use for all Date formatting in ShopwareClient
        ///</summary>
        public static string ToISO8601Format(this DateTime dateTime)
            => dateTime.ToString("o"); // yyy-MM-ddTHH\\:mm\\:ss.fffffffzzz
    }
}
