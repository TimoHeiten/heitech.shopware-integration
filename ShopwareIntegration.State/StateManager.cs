using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.Logging;

namespace heitech.ShopwareIntegration.State
{
    internal class StateManager : IStateManager
    {
        private readonly IStateManager _client;
        private readonly IStateManager _logger;

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
            var ctxt = LoggingData.PrepareLogging<T>(context);
            _ = await loggingCallback(_logger, ctxt);
            try
            {
                result = await callback(_client, ctxt);
                ctxt = onSuccessFactory(ctxt, result);
            }
            catch (System.Exception ex)
            {
                ctxt.AdditionalData[LoggingData.IS_ERROR] = ex;
                await loggingCallback(_logger, ctxt);
                throw;
            }
            await loggingCallback(_logger, LoggingData.PrepareLogging<T>(ctxt, false));

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

        public async Task<IEnumerable<T>> RetrievePage<T>(DataContext context) where T : DetailsEntity
        {
            IEnumerable<T> result = default!;
            var ctxt = LoggingData.PrepareLogging<T>(context);
            _ = await _logger.RetrievePage<T>(ctxt);
            try
            {
                result = await _client.RetrievePage<T>(ctxt);
                ctxt = DataContext.FromRetrievePage<T>(result, ctxt);
            }
            catch (System.Exception ex)
            {
                ctxt.AdditionalData["error"] = ex;
                _ = await _logger.RetrievePage<T>(ctxt);
                throw;
            }
            _ = await _logger.RetrievePage<T>(LoggingData.PrepareLogging<T>(ctxt, false));

            return result;
        }

        public Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
             => HandleAsync<T>(
                    context,
                    (s, c) => s.UpdateAsync<T>(c),
                    (l, c) => l.UpdateAsync<T>(c),
                    onSuccessFactory: (dt, result) => DataContext.FromPatchResult(result, dt)
                );
    }
}