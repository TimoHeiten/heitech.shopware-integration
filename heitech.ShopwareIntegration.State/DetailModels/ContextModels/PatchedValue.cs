namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Specify the patched Values for the UpdateOperation. Use an anonymous or specialized object 
    ///</summary>
    public sealed class PatchedValue : DetailsEntity
    {
        public object Values { get; }
        public DetailsEntity Source { get; private init; }
        private PatchedValue(DetailsEntity source, object patchedValues) => (Id, Source, Values) = (source.Id, source, patchedValues);

        // as an example
        // public static PatchedValue ProductUpdate(ProductDetails product, int increaseStockBy = 0, decimal? newPrice = null)
        // {
        //     var patch = new
        //     {
        //         stock = product.Stock + increaseStockBy,
        //         availableStock = product.AvailableStock + increaseStockBy,
        //         price = new
        //         {
        //             gross = newPrice ?? product.Price[0].Gross
        //         }
        //     };
        //
        //     return new(product.Id, patch);
        // }

        public static PatchedValue From<T>(T entity, object patchedValues) where T : DetailsEntity
            => new(entity, patchedValues);
    }
}