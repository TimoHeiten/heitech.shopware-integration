using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Cache
{
    ///<summary>
    /// Cache item that has a timer attached to unlist itself from the store.
    ///</summary>
    public sealed class CacheItem
    {
        private const int DUE_TIME_MS = 600000; // 10 min
        public DataContext Context { get; }
        private readonly Action<CacheItem> _unlist;
        private CacheItem(DataContext context, Action<CacheItem> unlist)
        {
            _unlist = unlist;
            Context = context;

            var disabledSignaling = System.Threading.Timeout.Infinite;
            var timer = new System.Threading.Timer((_) => _unlist(this), state: this, DUE_TIME_MS, disabledSignaling);
        }

        public static CacheItem Create(DataContext ctxt, Action<CacheItem> unlist) => new(ctxt, unlist);

        public RessourceId Key => Context.Id;
    }
}