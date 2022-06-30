using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.Cache
{
    public class CacheStorage : IStateManager
    {
        private readonly IStateManager _client;

        public CacheStorage(IStateManager client)
        {
            _client = client;
        }

        private readonly Dictionary<string, CacheItem> _cache = new();
        private readonly Dictionary<string, CacheItem> _pages = new();
        private void Unlist(CacheItem item) => _cache.Remove(item.Key);

        public async Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;
            var item = CacheItem.Create(context, Unlist);

            if (_cache.TryGetValue(item.Key, out CacheItem? cached))
                result = (T)cached!.Context.Entity;
            else
                result = await _client.RetrieveDetails<T>(context);

            // cache refresh
            _cache[item.Key] = cached!;
            return result!;
        }

        public async Task<IEnumerable<T>> RetrievePage<T>(DataContext pageRequest) where T : DetailsEntity
        {
            var item = CacheItem.Create(pageRequest, Unlist);
            if (_pages.TryGetValue(item.Key, out CacheItem? cached))
            {
                // cache refresh
                _pages[item.Key] = item;
                return cached.Context.Cast<T>().ToArray();
            }

            var result = await _client.RetrievePage<T>(pageRequest);
            _pages.Add(item.Key, item);

            return result;
        }

        public async Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;
            var item = CacheItem.Create(context, Unlist);

            if (_cache.TryGetValue(item.Key, out CacheItem? cached))
            { _ = (T)cached!.Context.Entity; }
            
            // remove
           InvalidateCacheForDelete<T>(context);
            result = await _client.DeleteAsync<T>(context);

            return result;
        }

        private void InvalidateCacheForDelete<T>(DataContext context)
            where T : DetailsEntity
        {
            DataContext found = null!;
            foreach (var page in _pages.Values)
            {
                var exists = page.Context.FirstOrDefault(x => x.Id == context.Id);
                if (exists != null)
                {
                    var filtered = page.Context.Where(x => x.Id != context.Id).ToArray();
                    found = DataContext.FromRetrievePage<T>(filtered, page.Context);
                    break;
                }
            }

            if (found is not null)
            {
                var item = CacheItem.Create(found, Unlist);
                _pages[item.Key] = item;
            }

        }

        public async Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;

            var item = CacheItem.Create(context, Unlist);
            result = await _client.CreateAsync<T>(context);
            // no need to check, the api will crash if we try to duplicate a key anyways
            _cache.Add(item.Key, item);

            return result;
        }

        public async Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;

            var item = CacheItem.Create(context, Unlist);
            result = await _client.UpdateAsync<T>(context);

            if (_cache.ContainsKey(item.Key))
                _cache.Add(item.Key, item);
            else // cache refresh
                _cache[item.Key] = item;

            return result;
        }
    }
}