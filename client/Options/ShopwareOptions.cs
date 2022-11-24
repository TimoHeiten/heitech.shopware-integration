namespace client.Options;

public sealed class ShopwareOptions
{
    public string BaseUrl { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}