using System;

namespace ShopwareIntegration.Filter
{
    internal class LimitExpression : IParameter
    {
        public int Limit { get; }
        public int? Start { get; }
        public LimitExpression(int limit, int? start)
            => (Limit, Start) = (limit, start);

        public object AsContent()
            => new { Limit, Start };
    }
}
