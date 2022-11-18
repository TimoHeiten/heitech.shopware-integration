using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.Cache;
using heitech.ShopwareIntegration.State.Integration;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.Logging;

namespace heitech.ShopwareIntegration.State
{
    public static class Factory
    {
        ///<summary>
        /// Supply a HttpClientConfiguration to create the ShopwareClient and Retrieve an IStateManager.
        /// Default values for the decorators lead to using the standard logger(Console) and default in Memory cached Client
        /// <para>Recommended approach if you just want to get up and running quickly without referring to the Api too much</para>
        ///</summary>
        public static async Task<IStateManager> CreateAsync(HttpClientConfiguration config,
                                                            IStateManager client = null!,
                                                            IStateManager logger = null!)
        {
            var shopwareClient = await CreateClientAsync(config);
            return new StateManager(
                logger ?? new Logger(s => System.Console.WriteLine(s)),
                client ?? new CacheStorage(new Client(shopwareClient))
            )
            {
                ShopwareClient = shopwareClient
            };
        }

        public static IStateManager SilentLogger() => new Logger((_) => { });

        /// <summary>
        /// Creates the underlying Client directly if you prefer to use the fewer abstraction and want to handle serialization etc. yourself
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Task<ShopwareClient> CreateClientAsync(HttpClientConfiguration configuration)
            => ShopwareClient.CreateAsync(configuration);
    }
}