namespace heitech.ShopwareIntegration.State.Api
{
    public class Paging
    {
        public int Amount { get; }
        public int Page { get; }

        public Paging(int page, int? amount = null)
            => (Page, Amount) = (page, amount ?? 100);

        public static Paging PageOne { get; } = new(1);
    }
}