using System;
using System.Net.Http;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests
{
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
            var entityPath = ModelToPathMap.GetUrlFromType<TModel>();
            var requestUri = $"{baseAddress}{entityPath}";

            if (!string.IsNullOrWhiteSpace(PathParameter))
                requestUri += $"/{PathParameter}";

            return requestUri;
        }

        public override string ToString()
        {
            return $"{Method} - {BuildPath("$baseAddress/")}";
        }
    }
}
