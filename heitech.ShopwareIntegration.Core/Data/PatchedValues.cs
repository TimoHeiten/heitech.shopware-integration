namespace heitech.ShopwareIntegration.Core.Data
{
    /// <summary>
    /// Use this to add the Patched Values for an update via Shopware Api.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PatchedValues<T>
        where T : class, IHasShopwareId
    {
        internal object Values { get; }
        public PatchedValues(object values)
            => Values = values;
    }
}