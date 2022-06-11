using System.Collections;
using System.Collections.Generic;
using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.ProductUseCases
{
    public class ProductsResult : IEnumerable<Product>
    {
        public int FromPage { get; }
        public int Amount { get; }
        private readonly IReadOnlyList<Product> _products;

        public ProductsResult(IReadOnlyList<Product> products, ProductPaging paging)
            => (FromPage, Amount, _products) = (paging.Page, paging.Amount, products);

        public IEnumerator<Product> GetEnumerator()
            => _products.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}