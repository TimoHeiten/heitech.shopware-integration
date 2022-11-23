using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Filters;
using Newtonsoft.Json;
using Xunit;

namespace heitech.ShopwareIntegration.Core.Tests;

public sealed class ReadRequestExtensionTests
{
    private const string EntityId = "42";
    private readonly ShopwareClientSpy _shopwareClient = new();

    [Theory]
    [MemberData(nameof(RequestsAndExpected))]
    public async Task GetByIdAsync_calls_with_Correct_HttpRequestMessage(Func<IShopwareClient, Task> act, HttpRequestMessage expected)
    {
        // Arrange
        expected.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Act
        await act(_shopwareClient);

        // Assert
        _shopwareClient.Captured.Should().BeEquivalentTo(expected);
    }

    // example Model
    [ModelUri("product")]
    private sealed class Product : IHasShopwareId
    {
        public string Name { get; } = "name";
        public string Id { get; } = Guid.NewGuid().ToString("N");
    }

    public static IEnumerable<object[]> RequestsAndExpected
    {
        get
        {
            // by Id
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.GetByIdAsync<Product>(EntityId)),
                new HttpRequestMessage
                {
                    Content = null,
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://sw6/api/product/42"),
                }
            };
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.GetByIdAsync<Product>(EntityId, "query")),
                new HttpRequestMessage
                {
                    Content = null,
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://sw6/api/product/42?query")
                }
            };
            
            // list requests
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.GetListAsync<Product>()),
                new HttpRequestMessage
                {
                    Content = null,
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://sw6/api/product")
                }
            };
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.GetListAsync<Product>()),
                new HttpRequestMessage
                {
                    Content = null,
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://sw6/api/product")
                }
            };
            
            // search 
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.SearchAsync<Product>(new FilterFake())),
                new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new FilterFake()), Encoding.ASCII, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://sw6/api/search/product")
                }
            };
            
            // search by id
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.SearchIds<Product>(new FilterFake())),
                new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new FilterFake()), Encoding.ASCII, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://sw6/api/search-ids/product")
                }
            };
        }
    }
    
    // NSubstitute would have generally worked, but this is easier to manage with the capture
    private sealed class ShopwareClientSpy : IShopwareClient
    {
        public HttpRequestMessage Captured = null!;
        public string BaseUrl => "https://sw6/api/";

        public Task<RequestResult<T>> SendAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken) 
            where T : ShopwareDataContainer
        {
            Captured = message;
            return Task.FromResult(RequestResult<T>.Success(default!));
        }

        public Task AuthenticateAsync(CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task<string> GetResponseContentAsync(HttpRequestMessage message, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public void Dispose()
            => throw new NotSupportedException();
    }

    private sealed class FilterFake : IFilter
    {
        public object Value { get; } = new { name = "name", @operator = "eq" };
    }
}