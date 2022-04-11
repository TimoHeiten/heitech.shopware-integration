using System;
using System.Net.Http;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests
{
    ///<summary>
    /// Abstract version of the Generic Shopware Web Request. Use the RequestFactory to create instances of a ShopwareRequest<T>
    ///</summary>
    public abstract class ShopwareRequest<TModel>
        where TModel : BaseModel
    {
        protected virtual string? PathParameter { get; } = null;
        protected abstract HttpMethod Method { get; }
        protected abstract HttpContent Content { get; }

        public HttpRequestMessage GetRequest(Uri baseAddress)
            =>  new(Method, BuildPath(baseAddress.ToString())) { Content = Content };

        private string BuildPath(string baseAddress)
        {
            var entityPath = ModelUri.GetUrlFromType<TModel>();
            var requestUri = $"{baseAddress}{entityPath}";

            if (!string.IsNullOrWhiteSpace(PathParameter))
                requestUri += $"/{PathParameter}";

            return requestUri;
        }

        public override string ToString()
            =>  $"{Method} - {BuildPath("$baseAddress/")}";
    }
}
