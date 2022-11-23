using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Filters;
using heitech.ShopwareIntegration.Core.Read;

namespace heitech.ShopwareIntegration.Core
{
    public static class ReadExtensions
    {
        /// <summary>
        /// Get a single Entity by its Id, as specified in the ModelUri Attribute for the Model.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="id">The id of the Entity you are looking for</param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the Entity if successful</returns>
        public static Task<RequestResult<DataObject<T>>> GetByIdAsync<T>(this ShopwareClient client, string id)
            where T : class, IHasShopwareId
        {
            return GetByIdAsync<T>(client, id, null);
        }

        /// <summary>
        /// Get a single Entity by its Id, as specified in the ModelUri Attribute for the Model.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="id">The id of the Entity you are looking for</param>
        /// <param name="query">The query string for this entities endpoint</param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the Entity if successful</returns>
        public static Task<RequestResult<DataObject<T>>> GetByIdAsync<T>(this ShopwareClient client, string id, string query)
            where T : class, IHasShopwareId
        {
            var request = GetByIdRequest<T>.Create(client, id);
            return request.ExecuteAsync();
        }


        /// <summary>
        /// Get a list of entities for the specified type. Be Aware that this might lead to a Shopware Serialization Error if too many Entities exist.
        /// <para>In that case use the queryString overload or the search Request with an appropriate filtering instead</para>
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>/// <returns>RequestResult´T holding the collection of Entities if successful</returns></returns>
        public static Task<RequestResult<DataArray<T>>> GetListAsync<T>(this ShopwareClient client)
            where T : class, IHasShopwareId
        {
            return client.GetListAsync<T>(null);
        }

        /// <summary>
        /// Get a list of entities for the specified type. Be Aware that this might lead to a Shopware Serialization Error if too many Entities exist.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="query">The query string specifying filters</param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the collection of Entities if successful</returns>
        public static Task<RequestResult<DataArray<T>>> GetListAsync<T>(this ShopwareClient client, string query)
            where T : class, IHasShopwareId
        {
            var request = GetListRequest<T>.Create(client, query);
            return request.ExecuteAsync();
        }

        /// <summary>
        /// Get a list of entities for the specified type with the specified filter. This is a POST.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="filter">The filter object. see https://shopware.stoplight.io/docs/store-api/cf710bf73d0cd-search-queries for more information</param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the collection of Entities if successful</returns>
        public static Task<RequestResult<DataArray<T>>> SearchAsync<T>(this ShopwareClient client, IFilter filter)
            where T : class, IHasShopwareId
        {
            var search = SearchRequest<T>.Create(client, filter);
            return search.ExecuteAsync();
        }

        /// <summary>
        /// Create a Filter from any object. Be careful to comply with the appropriate Filter semantics.
        /// <para>Or use the Filter in namespace heitech.ShopwareIntegration.Core.Filters</para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="anonymousObject"></param>
        /// <returns></returns>
        public static IFilter CreateFilterFromAnonymous(this ShopwareClient client, object anonymousObject)
            => FromObject(anonymousObject);
        /// <summary>
        /// Create a Filter from any object, for instance an anonymous one.
        /// <para>see </para>
        /// </summary>
        /// <param name="o">Object that represents the filter</param>
        /// <returns></returns>
        private static IFilter FromObject(object o) => new Filter {Value = o};

        private sealed class Filter : IFilter
        {
            public object Value { get; set; }
        }
    }
}