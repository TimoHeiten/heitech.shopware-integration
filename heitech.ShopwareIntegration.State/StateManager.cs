using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Integration;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.Logging;

namespace heitech.ShopwareIntegration.State
{
    internal class StateManager : IStateManager
    {
        private readonly IStateManager _client;
        private readonly IStateManager _logger;

        /// <summary>
        /// Use only internally to access the Lower Level functionality directly
        /// </summary>
        internal ShopwareClient ShopwareClient { get; init; } = default!;
        
        internal StateManager(IStateManager logger, IStateManager client)
        {
            _client = client;
            _logger = logger;
        }

        private async Task<T> HandleAsync<T>(DataContext context,
                                            Func<IStateManager, DataContext, Task<T>> callback,
                                            Func<IStateManager, DataContext, Task<T>> loggingCallback,
                                            Func<DataContext, T, DataContext> onSuccessFactory)
            where T : DetailsEntity
        {
            T result = default!;
            var localContext = context.PrepareLogging();
            _ = await loggingCallback(_logger, localContext);
            try
            {
                result = await callback(_client, localContext);
                localContext = onSuccessFactory(localContext, result);
            }
            catch (Exception ex)
            {
                localContext.PrepareError(ex);
                await loggingCallback(_logger, localContext);
                throw;
            }
            await loggingCallback(_logger, localContext.PrepareLogging(false));

            return result;
        }

        public Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity
            => HandleAsync
            (
                context,
                (s, c) =>
                {
                    var result = s.CreateAsync<T>(c);
                    return result!;
                },
                (l, c) => l.CreateAsync<T>(c),
                onSuccessFactory: (dt, _) => dt
            );

        public Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
            => HandleAsync(
                context,
                (s, c) => s.DeleteAsync<T>(c),
                (l, c) => l.DeleteAsync<T>(c),
                onSuccessFactory: (dt, result) => DataContext.FromDelete<T>(result, dt)
            );

        public Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
            => HandleAsync(
                context,
                (s, c) => s.RetrieveDetails<T>(c),
                (l, c) => l.RetrieveDetails<T>(c),
                onSuccessFactory: (dt, result) => DataContext.FromRetrieveDetails(result, dt)
            );

        public async Task<IEnumerable<T>> RetrievePage<T>(DataContext dataContext) where T : DetailsEntity
        {
            IEnumerable<T> result = default!;
            var context = dataContext.PrepareLogging();
            _ = await _logger.RetrievePage<T>(context);
            try
            {
                result = (await _client.RetrievePage<T>(context)).ToArray();
                context = DataContext.FromRetrievePage<T>(result, context);
            }
            catch (Exception ex)
            {
                context.PrepareError(ex);
                _ = await _logger.RetrievePage<T>(context);
                throw;
            }
            _ = await _logger.RetrievePage<T>(context.PrepareLogging(false));

            return result;
        }

        public Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
             => HandleAsync(
                    context,
                    (s, c) => s.UpdateAsync<T>(c),
                    (l, c) => l.UpdateAsync<T>(c),
                    onSuccessFactory: (dt, result) => DataContext.FromUpdateResult(result, dt)
                );
    }
}