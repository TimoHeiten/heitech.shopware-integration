using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core
{
    /// <summary>
    /// Extend ShopwareClient functionality for convenience methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Create a httpRequestMessage for interaction with the Shopware Api and the appropriate headers
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpRequestMessage CreateHttpRequest(this IShopwareClient client, string requestUri, HttpMethod method = null, object content = null)
        {
            var message = new HttpRequestMessage
            {
                Content = content?.AsJsonContent(),
                Method = method ?? HttpMethod.Get,
                RequestUri = new Uri($"{client.BaseUrl}{requestUri}")
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return message;
        }
        
        /// <summary>
        /// Convert object to HttpContent with correct Header for HttpRequestMessage
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static HttpContent AsJsonContent(this object body)
        {
            var json = JsonConvert.SerializeObject(body);
            return new StringContent(json, Encoding.ASCII, "application/json");
        }
    }
}