using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core.Mutate
{
    internal sealed class CreateRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly T _payload;
        private readonly ShopwareClient _client;
        private CreateRequest(ShopwareClient client, string uri, T payload)
        {
            _uri = uri;
            _payload = payload;
            _client = client;
        }

        /// <summary>
        /// Create a CreateRequest for the specified Model. Make sure that the Model supports the writeable Properties only.
        /// Or else the Request will fail. I.e. Have a model for creation and a model for reading an Entity.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        internal static CreateRequest<T> Create(ShopwareClient client, T payload)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            return new CreateRequest<T>(client, uri, payload);
        }

        /// <summary>
        /// Actually Execute the Create Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataEmpty>> ExecuteAsync()
            => _client.SendAsync<DataEmpty>(this);

        public static implicit operator HttpRequestMessage(CreateRequest<T> createRequest) =>
            createRequest._client.CreateHttpRequest(HttpUtility.UrlEncode(createRequest._uri), HttpMethod.Post, createRequest._payload);
    }
}