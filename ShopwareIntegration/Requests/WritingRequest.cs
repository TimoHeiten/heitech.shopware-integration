using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Requests;
using heitech.ShopwareIntegration.Models;
using heitech.ShopwareIntegration;
using heitech.ShopwareIntegration.Configuration;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

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

        private Task<RequestResult<DataEmpty>> RunAsync(string url, object body = null!, HttpMethod? method = null)
        {
            var httpRq = _client.CreateHttpRequest(url, body, method);
            return _client.SendAsync<DataEmpty>(httpRq);
        }

        ///<summary>
        /// Uses the HttpPatch to perform an Update. Provide the BaseEntity as the Payload argument. The Payload object is 
        /// supposed to include the updated Properties for this to take effect
        ///<para/>
        /// the Result is an empty Data Container since this returns a 204 StatusCode (No Content ) 
        ///</summary>
        public Task<RequestResult<DataEmpty>> Update(string id, object updated)
            => RunAsync($"{_url}/{id}", updated, HttpMethod.Patch);

        ///<summary>
        /// Uses the HttpPost to perform a Create. Provide the desired BaseEntity as the Payload argument. Make sure that all required Properties have a value and that all Properties at least exist.
        ///<para/>
        /// The Result is an empty Data Container since this returns a 204 StatusCode (No Content ) 
        ///</summary>
        public Task<RequestResult<DataEmpty>> Create(T payload)
            => RunAsync($"{_url}", payload, HttpMethod.Post);

        ///<summary>
        /// Uses the HttpDelete to remove a BaseEntity at the Server
        ///<para/>
        /// the Result is an empty Data Container since this returns a 204 StatusCode (No Content ) 
        ///</summary>
        public Task<RequestResult<DataEmpty>> Delete(T payload) => Delete(payload.Id);
        public Task<RequestResult<DataEmpty>> Delete(string id)
            => RunAsync($"{_url}/{id}", method: HttpMethod.Delete);
    }
}