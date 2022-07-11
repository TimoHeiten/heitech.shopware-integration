using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Cache
{
    ///<summary>
    /// Cache item that has a timer attached to unlist itself from the store.
    ///</summary>
    internal sealed class CacheItem : IDisposable
    {
        private const int DUE_TIME_MS = 600000; // 10 min
        public DataContext Context { get; }
        private readonly Action<CacheItem> _unlist;
        private readonly IDisposable _timer = null!;
        private bool disposedValue;

        private CacheItem(DataContext context, Action<CacheItem> unlist = null!)
        {
            _unlist = unlist;
            Context = context;

            if (unlist is not null)
            {
                var disabledSignaling = System.Threading.Timeout.Infinite;
                _timer = new System.Threading.Timer(
                    (_) => { _unlist(this); this.Dispose(); },
                    state: this,
                    DUE_TIME_MS,
                    disabledSignaling
                );
            }
        }

        // use for the key generation only
        public static CacheItem CreateTemp(DataContext ctxt) => new(ctxt);
        // use as the actual cache 
        public static CacheItem Create(DataContext ctxt, Action<CacheItem> unlist) => new(ctxt, unlist);

        public RessourceId Key => Context.Id;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}