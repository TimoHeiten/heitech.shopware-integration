using System.Reflection;

namespace heitech.ShopwareIntegration.State.Integration.Configuration;

/// <summary>
/// Specifies the endpoint Segment for the Shopware Api for any given Entity you want to Request. For Instance Products would have "product" as url Parameter to this attribute
/// see https://shopware.stoplight.io/docs/admin-api/ZG9jOjE0MzUyOTMz-entity-reference
/// </summary>
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