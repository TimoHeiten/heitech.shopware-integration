using System;
using System.Reflection;

namespace heitech.ShopwareIntegration.Configuration
{
    [AttributeUsage(validOn: AttributeTargets.Class, Inherited = false)]
    public class ModelUri : Attribute
    {
        public string Url { get; } = default!;

        public ModelUri(string url) => Url = url;

        public static string GetUrlFromType<T>()
        {
            var uri = typeof(T).GetCustomAttribute<ModelUri>() as ModelUri;
            return uri?.Url ?? throw new ArgumentNullException($"{typeof(T)} does not have a ModelUri Attribute applied. Make sure that it exists");
        }
    }
}
