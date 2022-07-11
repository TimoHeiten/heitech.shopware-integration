using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.Cache
{
    ///<summary>
    /// Cache Decorator for the StateManager. Updates and holds cached Context for up to 10 minutes.
    ///</summary>
    internal class CacheStorage : IStateManager
    {
        private readonly IStateManager _client;

        public CacheStorage(IStateManager client)
            => _client = client;

        protected readonly Dictionary<string, CacheItem> _cache = new();
        protected readonly Dictionary<string, CacheItem> _pages = new();
        private void Unlist(CacheItem detailsItem) => _cache.Remove(detailsItem.Key);

        public async Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
        {
            T result;
            var detailsItem = CacheItem.CreateTemp(context);

            if (_cache.TryGetValue(detailsItem.Key, out CacheItem? cached))
            {
                result = (T)cached!.Context.Entity;
                // cache refresh
                SubstituteCacheItem(cached, _cache);
            }
            else
            {
                result = await _client.RetrieveDetails<T>(context);
                var cachedResult = CacheItem.Create(DataContext.FromRetrieveDetails(result, context), Unlist);
                _cache.Add(cachedResult.Key, cachedResult);
            }

            return result!;
        }

        public async Task<IEnumerable<T>> RetrievePage<T>(DataContext dataContext) where T : DetailsEntity
        {
            var pageItem = CacheItem.CreateTemp(dataContext);
            if (_pages.TryGetValue(pageItem.Key, out CacheItem? cached))
            {
                // cache refresh
                SubstituteCacheItem(pageItem, _pages);
                return cached.Context.Cast<T>().ToArray();
            }

            var result = await _client.RetrievePage<T>(dataContext);
            var newPageContext = DataContext.FromRetrievePage<T>(result, dataContext);
            var cacheItem = CacheItem.Create(newPageContext, Unlist);
            _pages.Add(cacheItem.Key, cacheItem);

            return result;
        }

        public async Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;
            var detailsItem = CacheItem.CreateTemp(context);

            // remove from pages
            var cachedPage = FromPagesCache<T>(context);
            if (cachedPage is not null)
            {
                var filteredPages = cachedPage.Context.Where(x => x.Id != context.Id).ToArray();
                var cacheRefresh = DataContext.FromRetrievePage<T>(filteredPages, cachedPage.Context);
                SubstituteCacheItem(CacheItem.Create(cacheRefresh, Unlist), _pages);
            }

            if (_cache.ContainsKey(detailsItem.Key))
            {
                _cache[detailsItem.Key].Dispose();
                _cache.Remove(detailsItem.Key);
            }

            result = await _client.DeleteAsync<T>(context);

            return result;
        }

        public async Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;

            var detailsItem = CacheItem.Create(context, Unlist);
            result = await _client.CreateAsync<T>(context);
            // no need to check, the api will crash if we try to duplicate a key anyways
            _cache.Add(detailsItem.Key, detailsItem);

            #region cache refresh for pages
            CacheItem? pageCache = FromPagesCache<T>(context);
            if (pageCache is not null)
            {
                var updatedPage = pageCache.Context.Append(result).ToArray();
                var updatedContext = DataContext.FromRetrievePage<T>(updatedPage, context);
                SubstituteCacheItem(CacheItem.Create(updatedContext, Unlist), _pages);
            }
            #endregion

            return result;
        }

        public async Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;

            var detailsItem = CacheItem.CreateTemp(context);
            result = await _client.UpdateAsync<T>(context);
            SubstituteCacheItem(detailsItem, _cache);

            // cache refresh for pages
            CacheItem? pageItem = FromPagesCache<T>(context);
            if (pageItem is not null)
            {
                var updatedPage = pageItem.Context.Where(x => x.Id != context.Id).Append(result).ToArray();
                var updatedContext = DataContext.FromRetrievePage<T>(updatedPage, context);
                SubstituteCacheItem(CacheItem.Create(updatedContext, Unlist), _pages);
            }

            return result;
        }

        private CacheItem? FromPagesCache<T>(DataContext current)
           where T : DetailsEntity
        {
            var pageContext = CacheItem.CreateTemp(DataContext.GetPage<T>(current.PageNo));
            var exists = _pages.TryGetValue(pageContext.Key, out CacheItem? cachedPage);

            return cachedPage;
        }

        private void SubstituteCacheItem(CacheItem? newCacheItem, Dictionary<string, CacheItem> cache)
        {
            if (newCacheItem is not null)
            {
                cache[newCacheItem.Key].Dispose();
                cache[newCacheItem.Key] = newCacheItem;
            }
        }
    }
}