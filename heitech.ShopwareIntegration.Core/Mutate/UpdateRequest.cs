using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core.Mutate
{
    internal  sealed class UpdateRequest<T>
        where T : class, IHasShopwareId
    {
        private readonly string _uri;
        private readonly ShopwareClient _client;
        private readonly PatchedValues<T> _patchedValues;
        private UpdateRequest(ShopwareClient client, string uri, PatchedValues<T> patchedValues)
        {
            _uri = uri;
            _patchedValues = patchedValues;
            _client = client;
        }

        /// <summary>
        /// Create an UpdateRequest for the specified Model using Http.Patch.
        /// PatchedValues need to be an object akin to
        /// https://shopware.stoplight.io/docs/admin-api/ZG9jOjEyMzA4NTQ5-writing-entities#updating-entities
        /// with respect to the Entity Reference of the Shopware Api and the available properties
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <param name="patchedValues"></param>
        /// <returns></returns>
        internal static UpdateRequest<T> Create(ShopwareClient client, string id, PatchedValues<T> patchedValues)
        {
            var uri = ModelUri.GetUrlFromType<T>();
            return new UpdateRequest<T>(client, $"{uri}/{id}", patchedValues);
        }

        /// <summary>
        /// Actually Execute the Update Request
        /// </summary>
        /// <returns></returns>
        internal Task<RequestResult<DataEmpty>> ExecuteAsync()
            => _client.SendAsync<DataEmpty>(this);

        public static implicit operator HttpRequestMessage(UpdateRequest<T> updateRequest) =>
            updateRequest._client.CreateHttpRequest(HttpUtility.UrlEncode(updateRequest._uri), new HttpMethod("PATCH"), updateRequest._patchedValues);
    }
}