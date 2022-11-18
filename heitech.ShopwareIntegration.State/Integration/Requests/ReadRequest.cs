using System.Net.Http.Json;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;
using heitech.ShopwareIntegration.State.Integration.Models;
using heitech.ShopwareIntegration.State.Integration.Models.Data;

namespace heitech.ShopwareIntegration.State.Integration.Requests;

///<summary>
/// Specify a RequestObject that associates the generic Type as the Model. The Url is Taken from the ModelUri Attribute.
///</summary>
public sealed class ReadRequest<T>
    where T : BaseEntity
{
    private readonly string _url;
    private readonly ShopwareClient _client;

    ///<summary>
    /// Specify a RequestObject that associates the generic Type as the Model. The Url is Taken from the ModelUri Attribute.
    /// Supply the ShopwareClient to get Access to the shopware6 authorized web Requests
    ///</summary>
    internal ReadRequest(ShopwareClient client)
    {
        _url = ModelUri.GetUrlFromType<T>();
        _client = client;
    }

    private async Task<RequestResult<TData>> RunAsync<TData>(string url, HttpMethod? method = null, HttpContent? body = null)
    {
        var httpRq = _client.CreateHttpRequest(url, method, body);
        var rqResult = await _client.SendAsync<TData>(httpRq);

        return rqResult;
    }

    ///<summary>
    /// Find the specified Entity for this Request by Id
    ///<para/>
    /// On Success the RequestResult contains a DataObject Container of the requested Type
    ///</summary>
    public Task<RequestResult<DataObject<T>>> ExecuteGetAsync(string id, string? query = null)
    {
        var url = query is null ? $"{_url}/{id}" : $"{_url}/{id}?{query}";
        return RunAsync<DataObject<T>>($"{_url}/{id}");
    }

    ///<summary>
    /// Get All Instance of the specified Entity as a Collection
    ///<para/>
    /// On Success the RequestResult contains a DataArray Container of the requested Type
    ///</summary>
    public Task<RequestResult<DataArray<T>>> ExecuteListAsync(string? query = null)
    {
        var url = query is null ? $"{_url}" : $"{_url}?{query}";
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
    /// Get a List of all Ids for this Resource. Like with Search you can use Filter, Expand etc.
    ///<para/>
    /// On Success the RequestResult contains a DataObject Container of the requested Type
    ///</summary>
    public Task<RequestResult<DataArray<string>>> SearchIdsAsync(IFilter search)
        => RunAsync<DataArray<string>>($"search-ids/{_url}", HttpMethod.Post, JsonContent.Create(search.AsSearchInstance()));
}