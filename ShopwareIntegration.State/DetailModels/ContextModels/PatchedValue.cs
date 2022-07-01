using System;
using System.Linq;

namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Specify the patched Values for the UpdateOperation. Use an anonymous or specialized object 
    ///</summary>
    public sealed class PatchedValue : DetailsEntity
    {
        public object Values { get; }
        public object Result { get; set; } = null!;
        public PatchedValue(string id, object patchedValues) => (Id, Values) = (id, patchedValues);

        public static PatchedValue ProductUpdate(ProductDetails product, int increaseStockBy = 0, decimal? newPrice = null)
        {
            var patch = new
            {
                stock = product.Stock + increaseStockBy,
                availableStock = product.AvailableStock + increaseStockBy,
                price = new
                {
                    gross = newPrice ?? product.Price[0].Gross
                }
            };

            return new(product.Id, patch);
        }
        // todo make for other 3 models
    }
}