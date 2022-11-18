using System.Net.Http.Headers;
using System.Net.Http.Json;
using heitech.ShopwareIntegration.State.Integration.Models;
using heitech.ShopwareIntegration.State.Integration.Requests;

namespace heitech.ShopwareIntegration.State.Integration;

public static class Extensions
{
    /// <summary>
    /// Create a ReadRequest Instance for Access to the Get, Search, GetList and SearchIds abstractions of the Shopware API
    /// </summary>
    /// <param name="client"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ReadRequest<T> CreateReader<T>(this ShopwareClient client) 
        where T : BaseEntity => new(client);

    /// <summary>
    /// Create a WritingRequest instance for access to the POST, PATCH and DELETE abstractions of the Shopware API
    /// </summary>
    /// <param name="client"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WritingRequest<T> CreateWriter<T>(this ShopwareClient client) 
        where T : BaseEntity => new(client);

    /// <summary>
    /// Create an HttpRequestMessage for the Raw usage of the Authenticated ShopwareClient
    /// </summary>
    /// <param name="client"></param>
    /// <param name="uri"></param>
    /// <param name="model"></param>
    /// <param name="method"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    public static HttpRequestMessage CreateHttpRequest<TModel>(this ShopwareClient client, string uri, TModel model, HttpMethod? method = null)
    {
        //Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));
        HttpRequestMessage message = new()
        {
            Content = JsonContent.Create(model),
            Method = method ?? HttpMethod.Post,
            RequestUri = new Uri($"{client.BaseUrl}{uri}")
        };
        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return message;
    }

    /// <summary>
    /// Create an HttpRequestMessage for the Raw usage of the Authenticated ShopwareClient
    /// </summary>
    /// <param name="client"></param>
    /// <param name="uri"></param>
    /// <param name="method"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static HttpRequestMessage CreateHttpRequest(this ShopwareClient client, string uri, HttpMethod? method = null, HttpContent? content = null)
    {
        HttpRequestMessage message = new()
        {
            Content = content,
            Method = method ?? HttpMethod.Get,
            RequestUri = new Uri($"{client.BaseUrl}{uri}")
        };
        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return message;
    }
}