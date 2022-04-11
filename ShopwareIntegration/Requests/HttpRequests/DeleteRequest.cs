using System;
using System.Net.Http;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests.HttpRequests
{
    internal class DeleteRequest<TModel, TId> : ShopwareRequest<TModel>
        where TModel : BaseModel
    {
        protected override HttpMethod Method => HttpMethod.Delete;
        protected override HttpContent Content => null!; // delete has no body
        protected override string? PathParameter { get; }

        internal DeleteRequest(TId id)
            => PathParameter = $"{id}";
    }
}
