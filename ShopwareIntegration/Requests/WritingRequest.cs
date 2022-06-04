using System.Net.Http;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Requests;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests
{
    public class WritingRequest<T>
        where T : BaseEntity
    {
        private readonly string _url;
        private readonly ShopwareClient _client;

        public WritingRequest(ShopwareClient client)
        {
            this._url = ModelUri.GetUrlFromType<T>();
            this._client = client;
        }

        private Task<RequestResult<DataEmpty>> RunAsync(string url, T body = null!, HttpMethod? method = null)
        {
            var httpRq = _client.CreateHttpRequest(url, body, method);
            return _client.SendAsync<DataEmpty>(httpRq);
        }

        public Task<RequestResult<DataEmpty>> Update(T payload)
            => RunAsync($"{_url}/{payload.Id}", payload, HttpMethod.Patch);

        public Task<RequestResult<DataEmpty>> Create(T payload)
            => RunAsync($"{_url}", payload, HttpMethod.Post);

        public Task<RequestResult<DataEmpty>> Delete(T payload) => Delete(payload.Id);
        public Task<RequestResult<DataEmpty>> Delete(string id)
            => RunAsync($"{_url}/{id}", method: HttpMethod.Delete);
    }
}