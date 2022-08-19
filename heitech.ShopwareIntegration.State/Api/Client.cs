using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Integration;
using heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.Api
{
    public class Client : IStateManager
    {
        private readonly ShopwareClient _client;
        public Client(ShopwareClient client)
        {
            _client = client;
        }

        public async Task<T> CreateAsync<T>(DataContext dataContext) where T : DetailsEntity
        {
            var writer = _client.CreateWriter<T>();
            var rqResult = await writer.Create(dataContext.Entity);
            return rqResult.IsSuccess ? (T)dataContext.Entity : throw rqResult.Exception;
        }

        public async Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
        {
            if (!context.HasDelete())
                throw new InvalidOperationException("To Delete an Entity you need to create a DeleteContext first");

            var writer = _client.CreateWriter<T>();
            var rqResult = await writer.Delete(context.Id);
            return rqResult.IsSuccess ? (T)context.Entity : throw rqResult.Exception;
        }

        public async Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
        {
            var reader = _client.CreateReader<T>();
            var (exists, searchObj) = context.SearchExists<T>();
            if (exists)
            {
                var searchResult = await reader.SearchAsync(searchObj!.FromAnonymous());
                return searchResult.IsSuccess ? searchResult.Model.Data[0] : throw searchResult.Exception;
            }

            var (queryExists, query) = context.QueryExists<T>();
            var getResult = await reader.ExecuteGetAsync(context.Id, queryExists ? query! : null);

            return getResult.IsSuccess ? getResult.Model.Data : throw getResult.Exception;
        }

        public async Task<IEnumerable<T>> RetrievePage<T>(DataContext dataContext) where T : DetailsEntity
        {
            var reader = _client.CreateReader<T>();
            var result = await reader.SearchAsync(dataContext.GetFilter());

            return result.IsSuccess ? result.Model.Data : throw result.Exception;
        }

        public async Task<T> UpdateAsync<T>(DataContext dataContext) where T : DetailsEntity
        {
            var writer = _client.CreateWriter<T>();

            if (!dataContext.HasUpdate(out var update))
                throw new InvalidOperationException("Use an Update DataContext for an Update situation");

            var rqResult = await writer.Update(dataContext.Id, update!);

            // refresh the updated Entity, since the api does not deliver a new value -.-
            var details = await RetrieveDetails<T>(DataContext.GetDetail<T>(dataContext.Id, dataContext.PageNo,
                dataContext.AdditionalData!));

            return details is not null && rqResult.IsSuccess ? details : throw rqResult.Exception;
        }
    }
}