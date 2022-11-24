using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core
{
    public interface IShopwareClient : IDisposable
    {
        /// <summary>
        /// Shopware API BaseUrl
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Try to authenticate against the Shopware API
        /// </summary>
        Task AuthenticateAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Send a HttpRequestMessage. Includes retries and authenticates automatically. Handles different HttpMethods and the necessary Serialization
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<RequestResult<T>> SendAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken)
            where T : ShopwareDataContainer;

        /// <summary>
        /// For fine grained control of the Inputs/Outputs use this method. In all other Cases use the SendMessage or higher level abstractions of the Crud Package
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetResponseContentAsync(HttpRequestMessage message, CancellationToken cancellationToken);
    }
}