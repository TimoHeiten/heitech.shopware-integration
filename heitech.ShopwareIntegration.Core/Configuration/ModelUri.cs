using System;
using System.Reflection;

namespace heitech.ShopwareIntegration.Core.Configuration
{
    /// <summary>
    /// Specifies the endpoint Segment for the Shopware Api for any given Entity you want to Request. For Instance Products would have "product" as url Parameter to this attribute
    /// see https://shopware.stoplight.io/docs/admin-api/ZG9jOjE0MzUyOTMz-entity-reference
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, Inherited = false)]
    public sealed class ModelUri : Attribute
    {
        public string Url { get; }
        public ModelUri(string url) => Url = url;


        ///<summary>
        /// Read the URL for this Entity via the applied ModelUri type.
        ///</summary>
        public static string GetUrlFromType<T>()
        {
            var uri = typeof(T).GetCustomAttribute<ModelUri>();
            return uri?.Url ?? throw new ArgumentNullException($"{typeof(T)} does not have a ModelUri Attribute applied. Make sure that it exists");
        }
    }
}