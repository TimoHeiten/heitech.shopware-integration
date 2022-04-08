using System;
using System.Net.Http;
using System.Net.Http.Json;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Filters;

namespace ShopwareIntegration.Requests.HttpRequests
{
    internal class GetRequest<TModel, TId> : ShopwareRequest<TModel>
        where TModel : BaseModel, new()
    {
        protected override HttpMethod Method => HttpMethod.Get;
        protected override HttpContent Content { get; }

        protected override string? PathParameter { get; }

        internal GetRequest(TId id, FilterObject? filter = null)
        {
            PathParameter = $"{id}";
            Content = JsonContent.Create(filter ?? FilterObject.Empty);
        }
    }
}
