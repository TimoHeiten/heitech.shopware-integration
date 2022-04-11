using System;
using System.Net.Http;
using ShopwareIntegration.Configuration;
using ShopwareIntegration.Models;

namespace ShopwareIntegration.Requests.HttpRequests
{
    public class InfoRequest : ShopwareRequest<InfoModel>
    {
        public InfoRequest()
        { }

        protected override HttpMethod Method => HttpMethod.Get;

        protected override HttpContent Content => null!;
    }

    [ModelUri("_info/version")]
    public class InfoModel : BaseModel
    {
        public string Value { get; set; }
        public InfoModel()
        {
        }

        public override string TableName => string.Empty;
    }
}
