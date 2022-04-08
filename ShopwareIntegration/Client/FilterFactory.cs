using System;
using ShopwareIntegration.Models.Filters;

namespace ShopwareIntegration.Client
{
    public static class FilterFactory
    {
        public static FilterExpression EqualsFilter<TValue>(this ShopwareClient client, string property, TValue value)
            => new(property, "==", $"{value}");

        public static FilterExpression NotEqualsFilter<TValue>(this ShopwareClient client, string property, TValue value)
            => new(property, "!=", $"{value}");
    }
}
