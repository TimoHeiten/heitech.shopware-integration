using System;
using System.Linq;
using heitech.ShopwareIntegration.Filtering;

namespace heitech.ShopwareIntegration.ProductUseCases
{
    public sealed class ProductByIdParameter
    {
        public string Id { get; }
        public string? Query { get; }
        public IFilter? Filter { get; }
        public ProductByIdParameter(string id, string? query = null, IFilter? filter = null)
            => (Id, Query, Filter) = (id, query, filter);

        public static implicit operator ProductByIdParameter(string id) => new(id);
    }
}