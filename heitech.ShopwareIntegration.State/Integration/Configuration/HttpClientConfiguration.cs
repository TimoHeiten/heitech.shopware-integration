using System.Text.Json.Serialization;

namespace heitech.ShopwareIntegration.State.Integration.Configuration;

///<summary>
/// Configuration Object for the underlying Shopware HttpClient
///</summary>
public class HttpClientConfiguration
{
    public string BaseUrl { get; }
    public string ClientId { get; }
    public string UserName { get; }
    public string ClientSecret { get; }

    [JsonConstructor]
    public HttpClientConfiguration(string baseUrl, string clientId, string userName, string clientSecret)
        => (BaseUrl, ClientId, UserName, ClientSecret) = (baseUrl, clientId, userName, clientSecret);
}