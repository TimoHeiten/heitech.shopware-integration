using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Exceptions;
using ShopwareIntegration.Requests;

namespace ShopwareIntegration
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

            var credentialsCache = new CredentialCache {
                {
                    new Uri(configuration.BaseUrl.Replace("api", "")),
                    "Digest",
                    new NetworkCredential(configuration.UserName, configuration.Password)
                }
            };
            HttpClientHandler digestHandler = new()
            {
                Credentials = credentialsCache,
                UseDefaultCredentials = false,
                PreAuthenticate = true
            };

            HttpClient client = new(digestHandler) { BaseAddress = new Uri(configuration.BaseUrl) };
            client.DefaultRequestHeaders.Add("Authorization", configuration.CreateBasicAuthHeader());
            client.DefaultRequestHeaders.Add("ApiKey", configuration.ApiKey);

            return new(client);
        }

        public async Task<RequestResult<TModel>> SendRequestAsync<TModel>(ShopwareRequest<TModel> request, CancellationToken cancellationToken)
            where TModel : BaseModel
        {
            var httpRequest = request.GetRequest(_client.BaseAddress!);
            var response = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode is false)
                return RequestResult<TModel>.Failed(new ShopIntegrationRequestException((int)response.StatusCode, httpRequest, content));

            System.Console.WriteLine(content);

            try
            {
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
