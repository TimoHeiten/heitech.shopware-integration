using System;
using System.Net.Http;

namespace ShopwareIntegration.Models.Exceptions
{
    internal class ShopIntegrationRequestException : Exception
    {
        internal ShopIntegrationRequestException(int statusCode, HttpRequestMessage? requestMessage, string reason)
            : base($"Http Request '{requestMessage?.Method}-{requestMessage?.RequestUri}' returned non success Statuscode: {statusCode}{Environment.NewLine}'{reason}'")
        { }

        internal ShopIntegrationRequestException(Type type)
            : base($"Http Response Content was requested to be deserialized for type '{type}' but it resulted in a NULL reference")
        { }
    }
}
