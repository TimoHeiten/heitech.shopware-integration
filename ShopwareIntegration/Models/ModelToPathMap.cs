using System;
using System.Collections.Generic;

namespace ShopwareIntegration.Models
{
    public static class ModelToPathMap
    {

        static readonly IReadOnlyDictionary<Type, string> _map;
        static ModelToPathMap()
        {
            _map = new Dictionary<Type, string>()
            {
                [typeof(Address)] = "Addresses",
            };
        }

        public static string GetUrlFromType<T>()
            where T : BaseModel
        {
            if (!_map.TryGetValue(typeof(T), out string? url))
                throw new InvalidOperationException($"Found no entry for '{typeof(T)}' the '{nameof(ModelToPathMap)}' Dictionary. Consider adding it");

            return url!;
        }
    }
}
