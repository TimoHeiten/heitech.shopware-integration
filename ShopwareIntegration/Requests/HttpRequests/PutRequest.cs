using System.Net.Http;
using System.Net.Http.Json;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests.HttpRequests
{
    internal class PutRequest<TModel> : ShopwareRequest<TModel>
        where TModel : BaseModel
    {
        protected override HttpMethod Method => HttpMethod.Put;
        protected override HttpContent Content
        {
            get
            {
                _instance.Validate();
                return JsonContent.Create(_instance);
            }
        }

        private readonly TModel _instance;

        internal PutRequest(TModel instance)
            => _instance = instance;
    }
}
