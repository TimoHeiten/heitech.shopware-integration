using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core.Read
{
    /// <summary>
    /// ReadRequest
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class GetByIdRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly ShopwareClient _client;
        private GetByIdRequest(ShopwareClient client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        /// <summary>
        /// Create a GetRequest for the specified Model
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static GetByIdRequest<T> Create(ShopwareClient client, string id)
            => Create(client, id, null);

        /// <summary>
        /// Create a GetRequest for the specified Model with a QueryString
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static GetByIdRequest<T> Create(ShopwareClient client, string id, string query)
        {
            var builder = new StringBuilder();
            var uri = ModelUri.GetUrlFromType<T>();

            builder.Append(uri);
            builder.Append($"/{id}");

            if (query == null)
                return new GetByIdRequest<T>(client, builder.ToString());

            var q = query.Replace("?", "");
            builder.Append($"?{q}");

            return new GetByIdRequest<T>(client, builder.ToString());
        }

        /// <summary>
        /// Actually Execute the Get Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataObject<T>>> ExecuteAsync()
            => _client.SendAsync<DataObject<T>>(this);

        public static implicit operator HttpRequestMessage(GetByIdRequest<T> getByIdRequest) =>
            getByIdRequest._client.CreateHttpRequest(HttpUtility.UrlEncode(getByIdRequest._uri), HttpMethod.Get);
    }
}