using System.Net.Http;
using System.Net.Http.Json;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Filters;

namespace ShopwareIntegration.Requests.HttpRequests
{
    public class GetListRequest<TModel> : ShopwareRequest<TModel>
        where TModel : BaseModel
    {
        protected override HttpMethod Method => HttpMethod.Get;
        protected override HttpContent Content { get; }
        public GetListRequest(FilterObject? filter = null)
        {
            Content = JsonContent.Create(filter ?? FilterObject.Empty);
        }
    }
}
