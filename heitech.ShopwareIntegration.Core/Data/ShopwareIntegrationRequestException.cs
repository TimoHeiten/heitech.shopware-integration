using System;
using System.Net.Http;

namespace heitech.ShopwareIntegration.Core.Data
{
    internal sealed class ShopwareIntegrationRequestException : Exception
    {
        internal ShopwareIntegrationRequestException(int statusCode, HttpRequestMessage requestMessage, string reason)
            : base($"Http Request '{requestMessage?.Method}-{requestMessage?.RequestUri}' returned non success Statuscode: {statusCode}{Environment.NewLine}'{reason}'")
        { }

        internal ShopwareIntegrationRequestException(Type type)
            : base($"Http Response Content was requested to be deserialized for type '{type}' but it resulted in a NULL reference")
        { }

        internal ShopwareIntegrationRequestException(string msg)
            : base(msg)
        { }
    }
}
