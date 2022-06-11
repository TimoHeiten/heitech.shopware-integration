namespace heitech.ShopwareIntegration.ProductUseCases
{
    public class ProductPaging
    {
        public int Amount { get; }
        public int Page { get; }

        public ProductPaging(int page, int? amount = null)
            => (Page, Amount) = (page, amount ?? 100);
    }
}