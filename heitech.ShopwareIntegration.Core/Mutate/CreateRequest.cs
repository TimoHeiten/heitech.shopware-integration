using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core.Mutate
{
    internal sealed class CreateRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly T _payload;
        private readonly IShopwareClient _client;
        private CreateRequest(IShopwareClient client, string uri, T payload)
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
        internal static CreateRequest<T> Create(IShopwareClient client, T payload)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            return new CreateRequest<T>(client, uri, payload);
        }

        /// <summary>
        /// Actually Execute the Create Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataEmpty>> ExecuteAsync(CancellationToken cancellationToken)
            => _client.SendAsync<DataEmpty>(this, cancellationToken);

        public static implicit operator HttpRequestMessage(CreateRequest<T> createRequest) =>
            createRequest._client.CreateHttpRequest((createRequest._uri), HttpMethod.Post, createRequest._payload);
    }
}