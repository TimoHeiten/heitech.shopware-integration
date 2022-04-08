using System;

namespace ShopwareIntegration.Models.Filters
{
    internal class LimitExpression : IParameter
    {
        public int Limit { get; }
        public int? Start { get; }
        public LimitExpression(int limit, int? start)
            => (Limit, Start) = (limit, start);

        public object ToAnonymousFilterObject()
            => new { Limit, Start };
    }
}
