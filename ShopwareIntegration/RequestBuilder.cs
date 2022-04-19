using System;
using System.Net.Http;
using ShopwareIntegration.Filter;
using ShopwareIntegration.Requests;

namespace ShopwareIntegration
{
    ///<summary>
    /// Builder Pattern to construct a ShopwareRequest<T>
    ///</summary>
    public class RequestBuilder<T>
    {
        private string? uri;
        private IParameter? parameter;
        private HttpMethod httpMethod = HttpMethod.Get;
        private bool shouldAuthenticateRequest = false;

        private readonly ShopwareClient _client;

        public RequestBuilder(ShopwareClient client)
            => _client = client;

        public RequestBuilder<T> WithMethod(HttpMethod method)
        {
            httpMethod = method;
            return this;
        }

        public RequestBuilder<T> WithUri(string uri)
        {
            this.uri = uri;
            return this;
        }

        public RequestBuilder<T> WithParameter(IParameter parameter)
        {
            this.parameter = parameter;
            return this;
        }

        ///<summary>
        /// Use when you want to make sure the Request is Authenticated before it is send
        ///</summary>
        public RequestBuilder<T> WithExplicitAuthentication()
        {
            shouldAuthenticateRequest = true;
            return this;
        }

        public ShopwareRequest<T> Build()
        {
            Uri u = new($"{_client.Configuration.BaseUrl}{this.uri}");
            return new(_client, u, httpMethod, shouldAuthenticateRequest, parameter);
        }
    }
}
