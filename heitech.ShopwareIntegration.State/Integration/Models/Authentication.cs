using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.State.Integration.Configuration;

namespace heitech.ShopwareIntegration.State.Integration.Models;

///<summary>
/// Result Model
///</summary>
public class Authenticated
{
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = default!;
        
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;
}

///<summary>
/// Request Model
///</summary>
[ModelUri("oauth/token")]
public class Authenticate
{
    public const string Url = "oauth/token";
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = default!;

    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; } = "client_credentials";

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = default!;

    internal static Authenticate From(HttpClientConfiguration configuration)
        => new() { ClientId = configuration.ClientId, ClientSecret = configuration.ClientSecret };
}