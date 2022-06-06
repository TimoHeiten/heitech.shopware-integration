using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.Filtering
{
    public static class FilterConstants
    {
        public const string ASC = "asc";
        public const string DESC = "desc";

        public static string GetName<T>(this Expression<Func<T, object>> propertyExpression)
            where T : BaseEntity
        {
            var member = propertyExpression.Body as MemberExpression;
            var propInfo = member?.Member as PropertyInfo;
            if (propInfo is null)
                return "";

            var jsonName = propInfo.GetCustomAttribute<JsonPropertyNameAttribute>();

            string toCamelCase(string str)
                => str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return jsonName?.Name ?? toCamelCase(propInfo.Name);
        }
    }
}