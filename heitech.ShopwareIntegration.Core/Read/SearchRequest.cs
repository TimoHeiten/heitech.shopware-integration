using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IShopwareClient _client;
        private SearchRequest(IShopwareClient client, string uri, object filter)
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
        internal static SearchRequest<T> Create(IShopwareClient client, IFilter filter)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            
            return new SearchRequest<T>(client, $"search/{uri}", filter.Value);
        }

        /// <summary>
        /// Actually Execute the Get Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataArray<T>>> ExecuteAsync(CancellationToken cancellationToken)
            => _client.SendAsync<DataArray<T>>(this, cancellationToken);

        public static implicit operator HttpRequestMessage(SearchRequest<T> searchRequest) 
            => searchRequest._client.CreateHttpRequest(searchRequest._uri, HttpMethod.Post, searchRequest._filter);
    }
}