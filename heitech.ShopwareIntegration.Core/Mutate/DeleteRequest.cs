using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core.Mutate
{
    internal sealed class DeleteRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly IShopwareClient _client;
        private DeleteRequest(IShopwareClient client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        /// <summary>
        /// Create a DeleteRequest for the specified Model specified by its Id
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static DeleteRequest<T> Create(IShopwareClient client, string id)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            return new DeleteRequest<T>(client, $"{uri}/{id}");
        }

        /// <summary>
        /// Actually Execute the Delete Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataEmpty>> ExecuteAsync(CancellationToken cancellationToken)
            => _client.SendAsync<DataEmpty>(this, cancellationToken);

        public static implicit operator HttpRequestMessage(DeleteRequest<T> deleteRequest) =>
            deleteRequest._client.CreateHttpRequest(HttpUtility.UrlEncode(deleteRequest._uri), HttpMethod.Delete);
    }
}