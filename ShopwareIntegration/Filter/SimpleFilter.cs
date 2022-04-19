using System.Collections.Generic;

namespace ShopwareIntegration.Filter
{
    public class SimpleFilter : IParameter
    {
        private readonly Dictionary<string, object> _map;

        public SimpleFilter(Dictionary<string, object> map)
            => _map = map;

        public object AsContent()
            => _map;
    }
}
