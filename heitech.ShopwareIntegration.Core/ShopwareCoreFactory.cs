using System;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core
{
    /// <summary>
    /// Access to create a ShopwareClient
    /// </summary>
    public static class ShopwareCoreFactory
    {
        /// <summary>
        /// /// Create a new authenticated Instance by using the specified parameters for the Shopware Api
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="clientId"></param>
        /// <param name="userName"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static Task<IShopwareClient> CreateAsync(string baseUrl, string clientId, string userName,
            string clientSecret)
        {
            return CreateAsync(new HttpClientConfiguration(baseUrl, clientId, userName, clientSecret), CancellationToken.None);
        }

        /// <summary>
        /// Create a new authenticated Instance by using the specified HttpClientConfiguration
        /// </summary>
        /// <param name="clientConfiguration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<IShopwareClient> CreateAsync(HttpClientConfiguration clientConfiguration, CancellationToken cancellationToken)
        {
            if (clientConfiguration is null)
                throw new NullReferenceException($"The HttpConfiguration could not be loaded at: {typeof(ShopwareClient)} {nameof(CreateAsync)}");

            if (!clientConfiguration.IsValid())
                throw new ShopwareIntegrationRequestException($"{nameof(HttpClientConfiguration)} is not valid! Check if all Properties are set appropriately (non empty and valid)");

            var httpClient = clientConfiguration.CreateHttpClient();
            var shopwareClient = new ShopwareClient(httpClient)
            {
                Configuration = clientConfiguration
            };
            await shopwareClient.AuthenticateAsync(cancellationToken).ConfigureAwait(false);

            return shopwareClient;
        }
    }
}