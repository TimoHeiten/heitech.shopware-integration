using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.Cache
{
    ///<summary>
    /// Cache Decorator for the StateManager. Updates and holds cached Context for up to 10 minutes.
    ///</summary>
    public class CacheStorage : IStateManager
    {
        // todo instead of searching the page cache to update on delete, update or create... just use the supplied pageNo
        private readonly IStateManager _client;

        public CacheStorage(IStateManager client)
            => _client = client;

        protected readonly Dictionary<string, CacheItem> _cache = new();
        protected readonly Dictionary<string, CacheItem> _pages = new();
        private void Unlist(CacheItem detailsItem) => _cache.Remove(detailsItem.Key);

        public async Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;
            var detailsItem = CacheItem.Create(context, Unlist);

            if (_cache.TryGetValue(detailsItem.Key, out CacheItem? cached))
                result = (T)cached!.Context.Entity;
            else
                result = await _client.RetrieveDetails<T>(context);

            // cache refresh
            _cache[detailsItem.Key] = cached!;
            return result!;
        }

        public async Task<IEnumerable<T>> RetrievePage<T>(DataContext pageRequest) where T : DetailsEntity
        {
            var pageItem = CacheItem.Create(pageRequest, Unlist);
            if (_pages.TryGetValue(pageItem.Key, out CacheItem? cached))
            {
                // cache refresh
                _pages[pageItem.Key] = pageItem;
                return cached.Context.Cast<T>().ToArray();
            }

            var result = await _client.RetrievePage<T>(pageRequest);
            _pages.Add(pageItem.Key, pageItem);

            return result;
        }

        public async Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;
            var detailsItem = CacheItem.Create(context, Unlist);

            if (_cache.TryGetValue(detailsItem.Key, out CacheItem? cached))
            { _ = (T)cached!.Context.Entity; }

            // remove
            CacheItem? pageItem = FromPagesCache<T>(context);
            if (pageItem is not null)
                _pages.Remove(detailsItem.Key);

            if (_cache.ContainsKey(detailsItem.Key))
                _cache.Remove(detailsItem.Key);

            result = await _client.DeleteAsync<T>(context);

            return result;
        }

        private void InvalidateCacheForDelete<T>(DataContext context)
            where T : DetailsEntity
        {
            CacheItem? detailsItem = FromPagesCache<T>(context);
            if (detailsItem is not null)
                _pages.Remove(detailsItem.Key);
        }

        private CacheItem? FromPagesCache<T>(DataContext current)
            where T : DetailsEntity
        {
            DataContext found = null!;
            foreach (var page in _pages.Values)
            {
                var exists = page.Context?.FirstOrDefault(x => x.Id == current.Id);
                if (exists != null)
                {
                    var filtered = page.Context!.Where(x => x.Id != current.Id).ToArray();
                    found = DataContext.FromRetrievePage<T>(filtered, page.Context!);
                    break;
                }
            }

            return found is null ? null : CacheItem.Create(found, Unlist);
        }

        public async Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;

            var detailsItem = CacheItem.Create(context, Unlist);
            result = await _client.CreateAsync<T>(context);
            // no need to check, the api will crash if we try to duplicate a key anyways
            _cache.Add(detailsItem.Key, detailsItem);

            #region cache refresh for pages
            DataContext? pageContext = null!;
            foreach (var page in _pages.Values)
            {
                var exists = page.Context?.Any(x => x.Id == context.Id);
                if (exists is not null)
                {
                    var pageIncludesAddedEntity = page!.Context!.ToArray().Append(result);
                    pageContext = DataContext.FromRetrievePage<T>(pageIncludesAddedEntity, page.Context!);
                    break;
                }
            }

            if (pageContext is not null)
            {
                var pageItem = CacheItem.Create(pageContext, Unlist);
                _pages[pageItem.Key] = pageItem;
            }
            #endregion

            return result;
        }

        public async Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity
        {
            T result = null!;

            var detailsItem = CacheItem.Create(context, Unlist);
            result = await _client.UpdateAsync<T>(context);

            if (_cache.ContainsKey(detailsItem.Key))
                _cache.Add(detailsItem.Key, detailsItem);
            else // cache refresh
                _cache[detailsItem.Key] = detailsItem;

            // cache refresh for pages
            CacheItem? pageItem = FromPagesCache<T>(context);
            if (pageItem is not null)
                _pages[pageItem.Key] = pageItem;

            return result;
        }
    }
}