namespace heitech.ShopwareIntegration.Core.Mutate
{
    /// <summary>
    /// Use this to add the Patched Values for an update via Shopware Api.
    /// </summary>
    public class PatchedValues<T>
        where T : class, IHasShopwareId
    {
        internal object Values { get; }
        public PatchedValues(object values)
            => Values = values;
    }
}