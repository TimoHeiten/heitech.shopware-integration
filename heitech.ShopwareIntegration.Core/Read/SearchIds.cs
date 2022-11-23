using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Filters;

namespace heitech.ShopwareIntegration.Core.Read
{
    internal sealed class SearchIdsRequest<T> 
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly object _filter;
        private readonly ShopwareClient _client;

        private SearchIdsRequest(ShopwareClient client, string uri, object filter)
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
        internal static SearchIdsRequest<T> Create(ShopwareClient client, IFilter filter)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            
            return new SearchIdsRequest<T>(client, $"search-ids/{uri}", filter.Value);
        }

        /// <summary>
        /// Actually Execute the Get Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataArray<T>>> ExecuteAsync()
            => _client.SendAsync<DataArray<T>>(this);

        public static implicit operator HttpRequestMessage(SearchIdsRequest<T> searchIdsRequest) 
            => searchIdsRequest._client.CreateHttpRequest(HttpUtility.UrlEncode(searchIdsRequest._uri), HttpMethod.Post, searchIdsRequest._filter);
    }
}