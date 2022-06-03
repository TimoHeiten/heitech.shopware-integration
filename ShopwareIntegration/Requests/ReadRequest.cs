using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Data;

namespace ShopwareIntegration.Requests
{
    ///<summary>
    /// Specify a Requestobject that associates the generice Type as the Model. The Url is Taken from the ModelUri Attribute.
    ///</summary>
    public class ReadRequest<T>
        where T : BaseEntity
    {
        private readonly string _url;
        private readonly ShopwareClient _client;

        ///<summary>
        /// Specify a Requestobject that associates the generice Type as the Model. The Url is Taken from the ModelUri Attribute.
        /// Supply the ShopwareClient to get Access to the shopware6 authorized web Requests
        ///</summary>
        public ReadRequest(ShopwareClient client)
        {
            this._url = ModelUri.GetUrlFromType<T>();
            this._client = client;
        }

        private async Task<RequestResult<TData>> RunAsync<TData>(string url, HttpMethod? method = null, HttpContent? body = null)
        {
            var httpRq = _client.CreateHttpRequest(url, method, body);
            var rqResult = await _client.SendAsync<TData>(httpRq);
            // todo remove
            // just for checks
            // dynamic d = rqResult.Model;
            // if (rqResult.IsSuccess)
            // {
            //     System.Console.WriteLine(d!.Data.ToString());
            // }
            return rqResult;
        }

        ///<summary>
        /// Find the specified Enity for this Request by Id
        ///</summary>
        public Task<RequestResult<DataObject<T>>> ExecuteGetAsync(string id)
            => RunAsync<DataObject<T>>($"{_url}/{id}");

        ///<summary>
        /// Get All Instance of the specified Entity as a Collection
        ///</summary>
        public Task<RequestResult<DataArray<T>>> ExecuteListAsync()
            => RunAsync<DataArray<T>>(_url);

        ///<summary>
        /// Use the search object to filter, expand, page the specified entity etc.
        ///</summary>
        public Task<RequestResult<DataArray<T>>> SearchAsync(object search)
            => RunAsync<DataArray<T>>($"search/{_url}", HttpMethod.Post, JsonContent.Create(search));

        ///<summary>
        /// Get a List of all Ids for this Ressource. Like with Search you can use Filter, Expand etc.
        ///</summary>
        public Task<RequestResult<DataArray<string>>> SearchIdsAsync(object search)
            => RunAsync<DataArray<string>>($"search-ids/{_url}", HttpMethod.Post, JsonContent.Create(search));
    }
}