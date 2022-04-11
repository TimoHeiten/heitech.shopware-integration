using System.Collections.Generic;
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
        internal GetListRequest(FilterBuilder filter = null!)
        {
            var builder = filter ?? FilterBuilder.Empty;
            Content = JsonContent.Create(builder.BuildFilter());
        }
    }
}
