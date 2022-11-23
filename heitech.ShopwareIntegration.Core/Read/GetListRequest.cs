using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core.Read
{
    internal sealed class GetListRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly IShopwareClient _client;
        private GetListRequest(IShopwareClient client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        /// <summary>
        /// Create a GetRequest for the specified Model
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static GetListRequest<T> Create(IShopwareClient client)
            => Create(client, null);

        /// <summary>
        /// Create a GetRequest for the specified Model with a QueryString
        /// </summary>
        /// <param name="client"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static GetListRequest<T> Create(IShopwareClient client, string query)
        {
            var builder = new StringBuilder();
            var uri = ModelUri.GetUrlFromType<T>();
            builder.Append(uri);

            if (query == null)
                return new GetListRequest<T>(client, builder.ToString());

            var q = query.Replace("?", "");
            builder.Append($"?{q}");

            return new GetListRequest<T>(client, builder.ToString());
        }

        /// <summary>
        /// Actually Execute the Get Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataArray<T>>> ExecuteAsync(CancellationToken cancellationToken)
            => _client.SendAsync<DataArray<T>>(this, cancellationToken);

        public static implicit operator HttpRequestMessage(GetListRequest<T> getByIdRequest) =>
            getByIdRequest._client.CreateHttpRequest((getByIdRequest._uri), HttpMethod.Get);
    }
}