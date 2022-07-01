using System;
using System.Linq;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Ressource can be found here
    /// https://shopware.stoplight.io/docs/admin-api/c2NoOjE0MzUxMjcy-order
    ///</summary>
    [ModelUri("order")]
    public class Order : DetailsEntity
    {

        // all object types need to be looked up seperately and
        // to access those properties either extend this class OR
        // use the (slow and expensive) dynamic keyword e.g.:
        // var order = Deserialize<Order>(jsonString);
        // dynamic price = order.Price;
        // Console.WriteLine(price.Gross);
        public Order()
        { }

        [JsonPropertyName("versionId")]
        public string VersionId { get; set; } = default!;

        [JsonPropertyName("updatedById")]
        public string UpdatedById { get; set; } = default!;

        [JsonPropertyName("updatedBy")]
        public object UpdatedBy { get; set; } = default!;

        [JsonPropertyName("updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonPropertyName("transactions")]
        public object Transactions { get; set; } = default!;

        [JsonPropertyName("totalRounding")]
        public object TotalRounding { get; set; } = default!;

        [JsonPropertyName("taxStatus")]
        public string TaxStatus { get; set; } = default!;

        [JsonPropertyName("tags")]
        public object Tags { get; set; } = default!;

        [JsonPropertyName("stateMachineState")]
        public object StateMachineState { get; set; } = default!;

        [JsonPropertyName("stateId")]
        public string stateId { get; set; } = default!;

        [JsonPropertyName("shippingTotal")]
        public float ShippingTotal { get; set; }

        [JsonPropertyName("shippingCosts")]
        public object ShippingCosts { get; set; } = default!;

        [JsonPropertyName("salesChannelId")]
        public string SalesChannelId { get; set; } = default!;

        [JsonPropertyName("salesChannel")]
        public object SalesChannel { get; set; } = default!;

        [JsonPropertyName("ruleIds")]
        public string[] RuleIds { get; set; } = Array.Empty<string>();

        [JsonPropertyName("price")]
        public object Price { get; set; } = default!;

        [JsonPropertyName("positionPrice")]
        public float positionPrice { get; set; }

        [JsonPropertyName("orderNumber")]
        public string orderNumber { get; set; } = default!;

        [JsonPropertyName("orderDateTime")]
        public DateTime OrderDateTime { get; set; } 

        [JsonPropertyName("oderDate")]
        public string OderDate { get; set; } = default!;

        [JsonPropertyName("orderCustomer")]
        public object OrderCustomer { get; set; } = default!;

        [JsonPropertyName("lineItems")]
        public object LineItems { get; set; } = default!;

        [JsonPropertyName("languageID")]
        public string LanguageID { get; set; } = default!;

        [JsonPropertyName("language")]
        public object Language { get; set; } = default!;

        [JsonPropertyName("itemRounding")]
        public object itemRounding { get; set; } = default!;

        [JsonPropertyName("documents")]
        public object Documents { get; set; } = default!;

        [JsonPropertyName("deliveries")]
        public object Deliveries { get; set; } = default!;

        [JsonPropertyName("deepLinkCode")]
        public string deepLinkCode { get; set; } = default!;

        [JsonPropertyName("customerComment")]
        public string CustomerComment { get; set; } = default!;

        [JsonPropertyName("customFields")]
        public object customFields { get; set; } = default!;

        [JsonPropertyName("currencyId")]
        public string CurrencyId { get; set; } = default!;

        [JsonPropertyName("currencyFactor")]
        public string CurrencyFactor { get; set; } = default!;

        [JsonPropertyName("currency")]
        public object Currency { get; set; } = default!;

        [JsonPropertyName("createdBy")]
        public object CreatedBy { get; set; } = default!;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("campaignCode")]
        public string CampaignCode { get; set; } = default!;

        [JsonPropertyName("billingAddressVersionId")]
        public string BillingAddressVersionId { get; set; } = default!;

        [JsonPropertyName("billingAddressId")]
        public string BillingAddressId { get; set; } = default!;

        [JsonPropertyName("billingAddress")]
        public object BillingAddress { get; set; } = default!;

        [JsonPropertyName("autoIncrement")]
        public int AutoIncrement { get; set; }

        [JsonPropertyName("amountTotal")]
        public float AmountTotal { get; set; }

        [JsonPropertyName("amountNet")]
        public float AmountNet { get; set; }

        [JsonPropertyName("affiliateCode")]
        public string AffiliateCode { get; set; } = default!;

        [JsonPropertyName("addresses")]
        public object Addresses { get; set; } = default!;
    }
}