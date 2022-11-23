using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Filters;

namespace heitech.ShopwareIntegration.Core
{
    public static class ReadExtensions
    {
        /// <summary>
        /// Get a single Entity by its Id, as specified in the ModelUri Attribute for the Model.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="id">The id of the Entity you are looking for</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the Entity if successful</returns>
        public static Task<RequestResult<DataObject<T>>> GetByIdAsync<T>(this IShopwareClient client, string id, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
            => GetByIdAsync<T>(client, id, null, cancellationToken);

        /// <summary>
        /// Get a single Entity by its Id, as specified in the ModelUri Attribute for the Model.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="id">The id of the Entity you are looking for</param>
        /// <param name="query">The query string for this entities endpoint</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the Entity if successful</returns>
        public static Task<RequestResult<DataObject<T>>> GetByIdAsync<T>(this IShopwareClient client, string id, string query, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var message = client.CreateHttpRequest(BuildGetIdUrl<T>(id, query));
            return client.SendAsync<DataObject<T>>(message, cancellationToken);
        }

        private static string BuildGetIdUrl<T>(string id, string query)
        {
            var builder = new StringBuilder();
            var uri = ModelUri.GetUrlFromType<T>();

            builder.Append(uri);
            builder.Append($"/{id}");

            if (query == null) 
                return builder.ToString();

            var q = query.Replace("?", "");
            builder.Append($"?{q}");

            return builder.ToString();
        }

        /// <summary>
        /// Get a list of entities for the specified type. Be Aware that this might lead to a Shopware Serialization Error if too many Entities exist.
        /// <para>In that case use the queryString overload or the search Request with an appropriate filtering instead</para>
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>/// <returns>RequestResult´T holding the collection of Entities if successful</returns></returns>
        public static Task<RequestResult<DataArray<T>>> GetListAsync<T>(this IShopwareClient client, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
            => client.GetListAsync<T>(null, cancellationToken);

        /// <summary>
        /// Get a list of entities for the specified type. Be Aware that this might lead to a Shopware Serialization Error if too many Entities exist.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="query">The query string specifying filters</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the collection of Entities if successful</returns>
        public static Task<RequestResult<DataArray<T>>> GetListAsync<T>(this IShopwareClient client, string query, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var url = BuildGetListUrl<T>(query);
            var message = client.CreateHttpRequest(url, HttpMethod.Get);

            return client.SendAsync<DataArray<T>>(message, cancellationToken);
        }

        private static string BuildGetListUrl<T>(string query)
        {
            var builder = new StringBuilder();
            builder.Append(ModelUri.GetUrlFromType<T>());

            if (query == null)
                return builder.ToString();

            var q = query.Replace("?", "");
            builder.Append($"?{q}");

            return builder.ToString();
        }

        /// <summary>
        /// Get a list of entities for the specified type with the specified filter. This is a POST.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="filter">The filter object. see https://shopware.stoplight.io/docs/store-api/cf710bf73d0cd-search-queries for more information</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the collection of Entities if successful</returns>
        public static Task<RequestResult<DataArray<T>>> SearchAsync<T>(this IShopwareClient client, IFilter filter, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var message = client.CreateHttpRequest($"search/{ModelUri.GetUrlFromType<T>()}", HttpMethod.Post, filter.Value);
            return client.SendAsync<DataArray<T>>(message, cancellationToken);
        }

        /// <summary>
        /// Get a list of ids for the specified entity type with the specified filter. This is a POST.
        /// </summary>
        /// <param name="client">The authenticated ShopwareClient</param>
        /// <param name="filter">The filter object. see https://shopware.stoplight.io/docs/store-api/cf710bf73d0cd-search-queries for more information</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the collection of Entities if successful</returns>
        public static Task<RequestResult<DataArray<T>>> SearchIds<T>(this IShopwareClient client, IFilter filter, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var message = client.CreateHttpRequest($"search-ids/{ModelUri.GetUrlFromType<T>()}", HttpMethod.Post, filter.Value);
            return client.SendAsync<DataArray<T>>(message, cancellationToken);
        }
        
        /// <summary>
        /// Create a Filter from any object. Be careful to comply with the appropriate Filter semantics.
        /// <para>Or use the Filter in namespace heitech.ShopwareIntegration.Core.Filters</para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="anonymousObject"></param>
        /// <returns></returns>
        public static IFilter CreateFilterFromAnonymous(this IShopwareClient client, object anonymousObject)
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