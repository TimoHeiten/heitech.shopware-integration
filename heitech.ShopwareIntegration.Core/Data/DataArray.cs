using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core.Data
{
    ///<summary>
    /// Holds an Array of Type T for the deserialization of Response objects of the Shopware Api.
    ///</summary>
    public sealed class DataArray<T> : ShopwareDataContainer
    {
        [JsonProperty("data")]
        public IReadOnlyList<T> Data { get; set; }

        public static DataArray<T> BuildForInsert(T first, params T[] more)
            => new DataArray<T>() { Data = new [] { first }.Concat(more).ToList() };
    }

}
