using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Filters;
using ShopwareIntegration.Requests;
using ShopwareIntegration.Requests.HttpRequests;

namespace ShopwareIntegration.Client
{
    public static class RequestFactory
    {
        ///<summary>
        /// Create a GetRequest for a specific ModelType, its Id and an optional FilterObject
        ///</summary>
        public static ShopwareRequest<TModel> CreateGetRequest<TModel, TId>(this ShopwareClient client, TId id, FilterObject? filter = null)
            where TModel : BaseModel, new()
            => new GetRequest<TModel, TId>(id, filter);

        ///<summary>
        /// Create a GetListRequest to fetch all Items of this type with regards to an optional FilterObject
        ///</summary>
        public static ShopwareRequest<TModel> CreateGetListRequest<TModel, TId>(this ShopwareClient client, FilterObject? filter = null)
            where TModel : BaseModel, new()
            => new GetListRequest<TModel>(filter);

        public static ShopwareRequest<TModel> CreateDeleteRequest<TModel, TId>(this ShopwareClient client, TId id)
            where TModel : BaseModel
            => new DeleteRequest<TModel, TId>(id);

        public static ShopwareRequest<TModel> CreatePutRequest<TModel>(this TModel model)
            where TModel : BaseModel
            => new PutRequest<TModel>(model);

        public static ShopwareRequest<TModel> CreatePostRequest<TModel>(this TModel model)
            where TModel : BaseModel
            => new PostRequest<TModel>(model);
    }
}
