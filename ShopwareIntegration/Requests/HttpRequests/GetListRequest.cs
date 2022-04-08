using System.Net.Http;
using System.Net.Http.Json;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Filters;

namespace ShopwareIntegration.Requests.HttpRequests
{
    internal class GetListRequest<TModel> : ShopwareRequest<TModel>
        where TModel : BaseModel
    {
        protected override HttpMethod Method => HttpMethod.Get;
        protected override HttpContent Content { get; }
        internal GetListRequest(FilterObject? filter = null)
        {
            Content = JsonContent.Create(filter ?? FilterObject.Empty);
        }
    }
}
