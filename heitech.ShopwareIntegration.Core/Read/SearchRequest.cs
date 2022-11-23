using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Filters;

namespace heitech.ShopwareIntegration.Core.Read
{
    /// <summary>
    /// Search 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SearchRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly object _filter;
        private readonly ShopwareClient _client;
        private SearchRequest(ShopwareClient client, string uri, object filter)
        {
            _uri = uri;
            _filter = filter;
            _client = client;
        }

        /// <summary>
        /// Create a GetRequest for the specified Model with a QueryString
        /// </summary>
        /// <param name="client"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static SearchRequest<T> Create(ShopwareClient client, IFilter filter)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            
            return new SearchRequest<T>(client, $"search/{uri}", filter.Value);
        }

        /// <summary>
        /// Actually Execute the Get Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataArray<T>>> ExecuteAsync()
            => _client.SendAsync<DataArray<T>>(this);

        public static implicit operator HttpRequestMessage(SearchRequest<T> searchRequest) 
            => searchRequest._client.CreateHttpRequest(HttpUtility.UrlEncode(searchRequest._uri), HttpMethod.Post, searchRequest._filter);
    }
}