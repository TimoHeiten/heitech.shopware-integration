namespace ShopwareIntegration.Requests
{
    public class WritingRequest
    {
        private readonly string _url;
        private readonly ShopwareClient _client;

        public WritingRequest(string url, ShopwareClient client)
        {
            this._url = url;
            this._client = client;
        }

        // https://shopware.stoplight.io/docs/admin-api/ZG9jOjEyMzA4NTQ5-writing-entities
    }
}