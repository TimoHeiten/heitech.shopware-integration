using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace heitech.ShopwareIntegration.Core
{
    public static class Extensions
    {
        public static HttpRequestMessage CreateHttpRequest(this ShopwareClient client, string requestUri, HttpMethod method = null, object content = null)
        {
            var message = new HttpRequestMessage()
            {
                Content = content?.AsJsonContent(),
                Method = method ?? HttpMethod.Get,
                RequestUri = new Uri($"{client.BaseUrl}{requestUri}")
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return message;
        }

        public static HttpContent AsJsonContent(this object body)
        {
            var json = JsonConvert.SerializeObject(body);
            return new StringContent(json, Encoding.ASCII, "application/json");
        }
    }
}