using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.Models;
using ShopwareIntegration.Models.Data;
using heitech.ShopwareIntegration;
using heitech.ShopwareIntegration.Configuration;

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
        internal ReadRequest(ShopwareClient client)
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
            // dynamic d = rqResult.Model!;
            // if (rqResult.IsSuccess)
            // {
            //     System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(d.Data));
            // }
            return rqResult;
        }

        ///<summary>
        /// Find the specified Enity for this Request by Id
        ///<para/>
        /// On Success the RequestResult contains a DataObject Container of the requested Type
        ///</summary>
        public Task<RequestResult<DataObject<T>>> ExecuteGetAsync(string id, string? query = null)
        {
            string url = query is null ? $"{_url}/{id}" : $"{_url}/{id}?{query}";
            return RunAsync<DataObject<T>>($"{_url}/{id}");
        }

        ///<summary>
        /// Get All Instance of the specified Entity as a Collection
        ///<para/>
        /// On Success the RequestResult contains a DataArray Container of the requested Type
        ///</summary>
        public Task<RequestResult<DataArray<T>>> ExecuteListAsync(string? query = null)
        {
            string url = query is null ? $"{_url}" : $"{_url}?{query}";
            return RunAsync<DataArray<T>>(_url);
        }

        ///<summary>
        /// Use the search object to filter, expand, page the specified entity etc.
        ///<para/>
        /// On Success the RequestResult contains a DataObject Container of the requested Type
        ///</summary>
        public Task<RequestResult<DataArray<T>>> SearchAsync(IFilter search)
            => RunAsync<DataArray<T>>($"search/{_url}", HttpMethod.Post, JsonContent.Create(search.AsSearchInstance()));

        ///<summary>
        /// Get a List of all Ids for this Ressource. Like with Search you can use Filter, Expand etc.
        ///<para/>
        /// On Success the RequestResult contains a DataObject Container of the requested Type
        ///</summary>
        public Task<RequestResult<DataArray<string>>> SearchIdsAsync(IFilter search)
            => RunAsync<DataArray<string>>($"search-ids/{_url}", HttpMethod.Post, JsonContent.Create(search.AsSearchInstance()));
    }
}