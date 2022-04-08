using System;
using System.Net.Http;

namespace ShopwareIntegration.Models.Exceptions
{
    public class ShopIntegrationRequestException : Exception
    {
        public ShopIntegrationRequestException(int statusCode, HttpRequestMessage requestMessage)
            : base($"Http Request '{requestMessage.Method}-{requestMessage.RequestUri}' returned non success Statuscode: {statusCode}")
        { }

        public ShopIntegrationRequestException(Type type)
            : base($"Http Response Content was requested to be deserialized for type '{type}' but it resulted in a NULL reference")
        { }
    }
}
