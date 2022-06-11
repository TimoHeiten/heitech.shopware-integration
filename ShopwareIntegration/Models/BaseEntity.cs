using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace heitech.ShopwareIntegration.Models
{
    public abstract class BaseEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();

        protected BaseEntity()
            => Id = CreateId;

        public bool TryGetEntityFromAdditional<T>(out T t, string? name = null)
        {
            bool success = false;
            var key = name ?? ToCamelCase(typeof(T).Name);

            t = default!;
            success = AdditionalProperties.ContainsKey(key);
            System.Console.WriteLine($"key is contained? {success}");
            if (success)
            {
                t = JsonSerializer.Deserialize<T>(AdditionalProperties[key].GetRawText())!;
            }

            return success;
        }

        private static string ToCamelCase(string value)
        {
            var chars = new char[] { char.ToLower(value[0]) }.Concat(value.Skip(1)).ToArray();
            return new string(chars);
        }

        internal static string CreateId => Guid.NewGuid().ToString("N");
    }
}