using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace heitech.ShopwareIntegration.Core.Tests
{
    public sealed class RequestTests : IDisposable
    {
        private readonly MockHttpMessageHandler _httpHandler;
        private readonly ShopwareClient _sut;
        public RequestTests()
        {
            _httpHandler = new MockHttpMessageHandler();
            var configuration = new StubbedConfiguration(_httpHandler);
            _sut = new ShopwareClient(configuration.CreateHttpClient())
            {
                Configuration = configuration
            };
        }

        public static string ShopwareUri(string resource = "")
            => $"http://sw6-tests:4100/api/{resource}";

        private HttpClientConfiguration CreateConfig()
            => new StubbedConfiguration(_httpHandler)
            {
                BaseUrl = ShopwareUri(""),
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                UserName = "Sw-User"
            };

        private Authenticated SetupAuth()
        {
            var authUrl = ShopwareUri("oauth/token");
            var authenticated = new Authenticated()
            {
                AccessToken = "token",
                TokenType = "type",
                ExpiresIn = 600
            };
            _httpHandler.Expect(HttpMethod.Post, authUrl)
                .Respond(new StringContent(JsonConvert.SerializeObject(authenticated)));

            return authenticated;
        }

        [Fact]
        public async Task Create_Returns_An_Authenticated_Client()
        {
            // Arrange
            var clientConfiguration = CreateConfig();
            var authenticated = SetupAuth();

            // Act
            var shopwareClient = await ShopwareCoreFactory.CreateAsync(clientConfiguration, CancellationToken.None);

            // Assert
            shopwareClient.Should().NotBeNull();
            var casted = shopwareClient as ShopwareClient;
            casted.Should().NotBeNull();
            var authHeader = casted!.HttpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == "Authorization");
            authHeader.Should().NotBeNull();
            authHeader.Value.Single().Should().Contain("Bearer token");
            authHeader.Value.Single().Should().Contain(authenticated.AccessToken);
        }

        [Theory]
        [InlineData("http://localhost:400/api", "clientId", "clientSecret", "user", false)]
        [InlineData("http://localhost:400/api", "", "clientSecret", "user", true)]
        [InlineData("http://localhost:400/api", "clientId", "", "user", true)]
        [InlineData("http://localhost:400/api", "clientId", "clientSecret", "", true)]
        [InlineData("localhost:400/api", "clientId", "clientSecret", "user", true)]
        public async Task Create_Fails_On_Invalid_Configuration(string url,
            string clientId, string clientSecret, string userName, bool shouldThrow)
        {
            // Arrange
            var config = new StubbedConfiguration(url, clientId, userName, clientSecret, _httpHandler);

            // Act
            var act = () => ShopwareCoreFactory.CreateAsync(config, CancellationToken.None);

            // Assert
            if (shouldThrow)
            {
                await act.Should().ThrowAsync<ShopwareIntegrationRequestException>();
            }
            else
            {
                _ = SetupAuth();
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task Create_Throws_if_cannot_be_Deserialized()
        {
            // Arrange
            var authUrl = ShopwareUri("oauth/token");
            var authenticated = new Authenticated()
            {
                AccessToken = "token",
                TokenType = "type",
                ExpiresIn = 600
            };
            _httpHandler.Expect(HttpMethod.Post, authUrl)
                .Respond(new StringContent(JsonConvert.SerializeObject(new { SomeProp = "any value", SomeOtherProp = "no value" })));

            // Act
            var act = () => _sut.AuthenticateAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ShopwareIntegrationRequestException>();
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.BadRequest)]
        public async Task Create_Throws_If_It_Returns_Non_SuccessCode(HttpStatusCode statusCode)
        {
            // Arrange
            var authUrl = ShopwareUri("oauth/token");
            var authenticated = new Authenticated()
            {
                AccessToken = "token",
                TokenType = "type",
                ExpiresIn = 600
            };
            _httpHandler.Expect(HttpMethod.Post, authUrl)
                .Respond(statusCode);

            // Act
            var act = () => _sut.AuthenticateAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ShopwareIntegrationRequestException>();
        }

        private void ArrangeHttpRequestMessageSend(ProductStub? productStub = null)
        {
            _httpHandler.Expect(ProductUri)
                        .Respond(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(productStub ?? new ProductStub())));
        }

        private static string ProductUri => ShopwareUri("product?id=100");

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task NotAuthenticated_Client_Retries_Once_and_Fails_Afterwards(HttpStatusCode statusCode)
        {
            // Arrange
            _httpHandler.Expect(ProductUri)
                        .Respond(statusCode);

            _httpHandler.Expect(ShopwareUri("oauth/token"))
                        .Respond(HttpStatusCode.BadRequest); // any other failed success code will do

            var rqMessage = new HttpRequestMessage(HttpMethod.Get, ProductUri);

            // Act
            var requestResult = await _sut.SendAsync<DataEmpty>(rqMessage, CancellationToken.None);

            // Assert
            requestResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task FailsForNonAuthReason_ReturnsFailed_Result()
        {
            // Arrange
            _httpHandler.Expect(ProductUri)
                        .Respond(HttpStatusCode.InternalServerError);
            var rqMessage = new HttpRequestMessage(HttpMethod.Get, ProductUri);

            // Act
            var requestResult = await _sut.SendAsync<DataEmpty>(rqMessage,CancellationToken.None);

            // Assert
            requestResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task NotAuthenticated_Client_Retries_Once_and_Works_Afterwards()
        {
            // Arrange
            _httpHandler.Expect(ProductUri)
                        .Respond(HttpStatusCode.Forbidden);
            _ = SetupAuth();
            _httpHandler.Expect(ProductUri)
                        .Respond(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new { })));

            var rqMessage = _sut.CreateHttpRequest(ProductUri);

            // Act
            var requestResult = await _sut.SendAsync<DataEmpty>(rqMessage,CancellationToken.None);

            // Assert
            requestResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Get_Works_and_Returns_Expected_DataContainer()
        {
            // Arrange
            _httpHandler.Expect(ProductUri).WithHeaders("Accept", "application/json")
                        .Respond(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new { data = new ProductStub() })));

            var rqMessage = _sut.CreateHttpRequest(ProductUri);

            // Act
            var requestResult = await _sut.SendAsync<DataObject<ProductStub>>(rqMessage, CancellationToken.None);

            // Assert
            requestResult.IsSuccess.Should().BeTrue();
            requestResult.Model.Data.Should().BeEquivalentTo(new ProductStub());
        }

        [Fact]
        public async Task Content_is_null_Returns_Request_Failed()
        {
            // Arrange
            _httpHandler.Expect(ProductUri)
                        .Respond(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(null)));

            var rqMessage = _sut.CreateHttpRequest(ProductUri);

            // Act
            var requestResult = await _sut.SendAsync<DataObject<ProductStub>>(rqMessage, CancellationToken.None);

            // Assert
            requestResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Api_Throwing_Exception_Returns_RequestFailedResult()
        {
            // Arrange
            _httpHandler.Expect(ProductUri)
                        .Throw(new Exception());

            var rqMessage = _sut.CreateHttpRequest(ProductUri);

            // Act
            var rqResult = await _sut.SendAsync<DataObject<ProductStub>>(rqMessage, CancellationToken.None);

            // Assert
            rqResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Deserialization_Throws_Returns_Request_Failed_Result()
        {
            // Arrange
            _httpHandler.Expect(ProductUri)
                        .Respond(HttpStatusCode.OK, new StringContent("this aint no { json {{{{}}'''',,,//"));

            var rqMessage = _sut.CreateHttpRequest(ProductUri);

            // Act
            var rqResult = await _sut.SendAsync<DataObject<ProductStub>>(rqMessage, CancellationToken.None);

            // Assert
            rqResult.IsSuccess.Should().BeFalse();
        }

        public void Dispose()
            => _httpHandler.VerifyNoOutstandingExpectation();

        private sealed class ProductStub
        {
            public int Id { get; set; } = 1;
            public string Name { get; set; } = "My-Product";
        }

        private sealed class StubbedConfiguration : HttpClientConfiguration
        {
            private readonly MockHttpMessageHandler _messageHandler;

            public StubbedConfiguration(MockHttpMessageHandler messageHandler)
                => _messageHandler = messageHandler;

            public StubbedConfiguration(string url, string clientId, string username, string clientSecret, MockHttpMessageHandler messageHandler)
                : base(url, clientId, username, clientSecret)
                => _messageHandler = messageHandler;

            internal override HttpClient CreateHttpClient()
            {
                var client = _messageHandler.ToHttpClient();
                client.BaseAddress = new Uri("http://sw6-tests:4100/api/");
                return client;
            }
        }
    }
}