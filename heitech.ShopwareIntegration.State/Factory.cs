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
        ///</summary>
        public static async Task<IStateManager> CreateAsync(HttpClientConfiguration config,
                                                            IStateManager client = null!,
                                                            IStateManager logger = null!)
        {
            var shopwareClient = await ShopwareClient.CreateAsync(config);
            return new StateManager(
                    logger ?? new Logger(s => System.Console.WriteLine(s)), 
                    client ?? new CacheStorage(new Client(shopwareClient))
            );
        }

        public static IStateManager SilentLogger() => new Logger((_) => { });
    }
}