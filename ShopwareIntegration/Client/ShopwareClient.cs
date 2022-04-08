using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Exceptions;
using ShopwareIntegration.Requests;

namespace ShopwareIntegration.Client
{
    ///<summary>
    /// HttpClient Abstraction for the Shopware REST API Integration
    ///</summary>
    public class ShopwareClient
    {
        private readonly HttpClient _client;
        private ShopwareClient(HttpClient client)
            => _client = client;

        public static async Task<ShopwareClient> CreateAsync()
        {
            var configuration = await Configuration.HttpClientConfiguration.LoadAsync().ConfigureAwait(false);
            if (configuration is null)
                throw new NullReferenceException($"The HttpConfiguration could not be loaded at: {typeof(ShopwareClient)} {nameof(CreateAsync)}");

            // todo: remove the trustingHandler, only for dev mode
            HttpClientHandler trustingHandler = new();
            trustingHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // !

            HttpClient client = new(trustingHandler) { BaseAddress = new Uri(configuration.BaseUrl) };
            client.DefaultRequestHeaders.Add("Authorization", configuration.CreateBasicAuthHeader());
            client.DefaultRequestHeaders.Add("ApiKey", configuration.ApiKey);

            return new(client);
        }

        public async Task<RequestResult<TModel>> SendRequestAsync<TModel>(ShopwareRequest<TModel> request, CancellationToken cancellationToken)
            where TModel : BaseModel
        {
            HttpRequestMessage httpRequest = request.GetRequest(_client.BaseAddress!);
            System.Console.WriteLine(httpRequest.RequestUri);
            HttpResponseMessage response = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode is false)
                return RequestResult<TModel>.Failed(new ShopIntegrationRequestException((int)response.StatusCode, httpRequest));

            try
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                TModel? model = System.Text.Json.JsonSerializer.Deserialize<TModel>(content);
                return model is not null
                       ? RequestResult<TModel>.Success(model!)
                       : RequestResult<TModel>.Failed(new ShopIntegrationRequestException(typeof(TModel)));
            }
            catch (System.Exception ex)
            {
                return RequestResult<TModel>.Failed(ex);
            }
        }
    }
}
