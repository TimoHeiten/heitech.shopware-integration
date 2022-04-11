using System;
using System.Net.Http;
using System.Net.Http.Json;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests.HttpRequests
{
    internal class PostRequest<TModel> : ShopwareRequest<TModel> 
        where TModel : BaseModel
    {
        protected override HttpMethod Method => HttpMethod.Post;
        protected override HttpContent Content
        {
            get
            {
                _instance.Validate();
                return JsonContent.Create(_instance);
            }
        }

        private readonly TModel _instance;
        public PostRequest(TModel instance)
            => _instance = instance;

    }
}
