using heitech.ShopwareIntegration.Configuration;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.Cache;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.Logging;
using ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State
{
    public static class Factory
    {
        public static async Task<IStateManager> CreateAsync(HttpClientConfiguration config,
                                                             ICache cache = null!,
                                                             IStateManager client = null!,
                                                             IStateManager logger = null!)
        {
            var shopwareClient = await ShopwareClient.CreateAsync(config);
            client ??= new Client(shopwareClient);


            return new StateManager(
                    logger ?? new Logger(s => System.Console.WriteLine(s)), 
                    client ?? new CacheStorage(new Client(shopwareClient)));
        }
    }
}