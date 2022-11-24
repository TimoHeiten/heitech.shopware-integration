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
using Newtonsoft.Json;
using Xunit;

namespace heitech.ShopwareIntegration.Core.Tests;

public sealed class WriteExtensionRequestTests
{
    private const string EntityId = "42";
    private readonly ShopwareClientSpy _shopwareClient = new();

    [Theory]
    [MemberData(nameof(RequestsAndExpected))]
    public async Task WriteExtension_Calls_Send_Correct_HttpRequestMessage(Func<IShopwareClient, Task> act, HttpRequestMessage expected)
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
            // Create
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.CreateAsync(new Product())),
                new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new Product()), Encoding.ASCII, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://sw6/api/product"),
                }
            };
            // Update
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.UpdateAsync(EntityId, new PatchedValues<Product>(new { Name = "name-2" } ))),
                new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { Name = "name-2" }), Encoding.ASCII, "application/json"),
                    Method = new HttpMethod("PATCH"),
                    RequestUri = new Uri("https://sw6/api/product/42")
                }
            };

            // Delete
            yield return new object[]
            {
                new Func<IShopwareClient, Task>(c => c.DeleteAsync<Product>(EntityId)),
                new HttpRequestMessage
                {
                    Content = null,
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri("https://sw6/api/product/42")
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
}