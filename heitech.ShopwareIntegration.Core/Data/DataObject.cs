using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core.Data
{
    ///<summary>
    /// For a single Object result from the Shopware Api wrap it in this Object
    ///</summary>
    public sealed class DataObject<T> : ShopwareDataContainer
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
