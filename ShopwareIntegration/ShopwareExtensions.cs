namespace ShopwareIntegration
{
    public static class ShopwareExtensions
    {
        public static RequestBuilder<T> BuilderFromClient<T>(this ShopwareClient client)
            => new(client);
    }
}
