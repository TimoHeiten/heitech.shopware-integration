using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Cache
{
    ///<summary>
    /// Cache item that has a timer attached to unlist itself from the store.
    ///</summary>
    public sealed class CacheItem
    {
        private const int DUE_TIME_MS = 600000; // 10 min
        internal DataContext Context { get; }
        private readonly Action<CacheItem> _unlist;
        private CacheItem(DataContext context, Action<CacheItem> unlist)
        {
            _unlist = unlist;
            Context = context;

            var disabledSignaling = System.Threading.Timeout.Infinite;
            var timer = new System.Threading.Timer((_) => _unlist(this), state: this, DUE_TIME_MS, disabledSignaling);
        }

        internal static CacheItem Create(DataContext ctxt, Action<CacheItem> unlist) => new(ctxt, unlist);

        internal RessourceId Key => Context.Id;
    }
}