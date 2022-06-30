using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.Logging
{
    public class Logger : IStateManager
    {
        private readonly Action<string> _logAction;
        public Logger(Action<string> logAction)
            => _logAction = (s) => logAction($"[{DateTime.Now}] -- {s}");

        private void Log<T>(Func<bool> isEntry, string methName, string id, int PageNo)
        {
            Func<string, string> generateMsg = entryOrExit => $"{entryOrExit}: {nameof(methName)} with 'Id: {id}' - 'PageNo: {PageNo}' - 'Type: {typeof(T).Name}'";
            if (isEntry())
                _logAction(generateMsg("enters"));
            else
                _logAction(generateMsg("leaves"));
        }

        public Task<T> CreateAsync<T>(DataContext dataContext) where T : DetailsEntity
        {
            Log<T>(dataContext.ReadIsEntry<T>, nameof(CreateAsync), dataContext.Entity.Id, dataContext.PageNo);
            return Task.FromResult((T)dataContext.Entity);
        }

        public Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
        {
            Log<T>(context.ReadIsEntry<T>, nameof(DeleteAsync), context.Entity.Id, context.PageNo);
            return Task.FromResult((T)context.Entity);
        }

        public Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
        {
            Log<T>(context.ReadIsEntry<T>, nameof(RetrieveDetails), context.Entity.Id, context.PageNo);
            return Task.FromResult((T)context.Entity);
        }

        public Task<IEnumerable<T>> RetrievePage<T>(DataContext context) where T : DetailsEntity
        {
           Log<T>(context.ReadIsEntry<T>, nameof(RetrievePage), context.Entity.Id, context.PageNo);
           return Task.FromResult<IEnumerable<T>>(context.Cast<T>().ToArray());
        }

        public Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
        {
            Log<T>(context.ReadIsEntry<T>, nameof(UpdateAsync), context.Entity.Id, context.PageNo);
            return Task.FromResult((T)context.Entity);
        }
    }
}