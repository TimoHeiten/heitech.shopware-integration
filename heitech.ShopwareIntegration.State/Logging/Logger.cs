using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.Logging
{
    internal class Logger : IStateManager
    {
        private readonly Action<string> _logAction;
        internal Logger(Action<string> logAction)
            => _logAction = (s) => logAction($"[{DateTime.Now}] -- {s}");

        private void Log<T>(DataContext context, string methName, string id, int PageNo)
            where T : DetailsEntity
        {
            Exception? ex = context.GetError();
            if (ex is not null)
            {
                _logAction($" {methName} with 'Id: {id}' - 'PageNo: {PageNo}' - 'Type: {typeof(T).Name}' threw:{Environment.NewLine}{ex}");
                return;
            }


            Func<string, string> generateMsg = entryOrExit => $"{entryOrExit}: {methName} with 'Id: {id}' - 'PageNo: {PageNo}' - 'Type: {typeof(T).Name}'";
            if (context.ReadIsEntry<T>())
                _logAction(generateMsg("enters"));
            else
                _logAction(generateMsg("exits"));
        }

        public Task<T> CreateAsync<T>(DataContext dataContext) where T : DetailsEntity
        {
            Log<T>(dataContext, nameof(CreateAsync), dataContext.Id, dataContext.PageNo);
            return Task.FromResult((T)dataContext.Entity);
        }

        public Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
        {
            Log<T>(context, nameof(DeleteAsync), context.Id, context.PageNo);
            return Task.FromResult((T)context.Entity);
        }

        public Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
        {
            Log<T>(context, nameof(RetrieveDetails), context.Id, context.PageNo);
            return Task.FromResult((T)context.Entity);
        }

        public Task<IEnumerable<T>> RetrievePage<T>(DataContext dataContext) where T : DetailsEntity
        {
            var contextForId = DataContext.GetPage<T>(dataContext.PageNo, dataContext.AdditionalData);
            Log<T>(dataContext, nameof(RetrievePage), contextForId.Id, dataContext.PageNo);
            return Task.FromResult<IEnumerable<T>>(dataContext.Cast<T>().ToArray());
        }

        public Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
        {
            Log<T>(context, nameof(UpdateAsync), context.Id, context.PageNo);
            return Task.FromResult((T)context.Entity);
        }
    }
}