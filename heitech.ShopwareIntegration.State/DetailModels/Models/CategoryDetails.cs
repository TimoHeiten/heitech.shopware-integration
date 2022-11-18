using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.State.Integration.Configuration;

namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Category details object
    /// <para/>
    /// Ressource can be found at https://shopware.stoplight.io/docs/admin-api/c2NoOjE0MzUxMjEx-category
    ///</summary>
    [ModelUri("category")]
    public class CategoryDetails : DetailsEntity
    {
        // all object types need to be looked up separately and
        // to access those properties either extend this class OR
        // use the (slow and expensive) dynamic keyword e.g.:
        // var order = Deserialize<OrderDetails>(jsonString);
        // dynamic price = order.Price;
        // Console.WriteLine(price.Gross);
        public CategoryDetails()
        { }

        [JsonPropertyName("active")]
        public bool? Active { get; set; }

        [JsonPropertyName("afterCategoryId")]
        public string AfterCategoryId { get; set; } = default!;

        [JsonPropertyName("afterCategoryVersionId")]
        public string AfterCategoryVersionId { get; set; } = default!;

        [JsonPropertyName("autoIncrement")]
        public long? AutoIncrement { get; set; }

        [JsonPropertyName("breadCrumb")]
        public string[] BreadCrumb { get; set; } = default!;

        [JsonPropertyName("childCount")]
        public long? ChildCount { get; set; } = default!;

        [JsonPropertyName("children")]
        public object Children { get; set; } = default!;

        [JsonPropertyName("cmsPage")]
        public string CmsPage { get; set; } = default!;

        [JsonPropertyName("cmsPageId")]
        public string CmsPageId { get; set; } = default!;

        [JsonPropertyName("cmsPageVersionId")]
        public string CmsPageVersionId { get; set; } = default!;

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; } = default!;

        [JsonPropertyName("customFields")]
        public object CustomFields { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("displayNestedProducts")]
        public bool? DisplayNestedProducts { get; set; }

        [JsonPropertyName("externalLink")]
        public string ExternalLink { get; set; } = default!;

        [JsonPropertyName("footerSlesChannels")]
        public object FooterSlesChannels { get; set; } = default!;

        [JsonPropertyName("internalLink")]
        public string InternalLink { get; set; } = default!;

        [JsonPropertyName("keywords")]
        public string Keywords { get; set; } = default!;

        [JsonPropertyName("level")]
        public long? Level { get; set; } = default!;

        [JsonPropertyName("linkNewTab")]
        public bool? LinkNewTab { get; set; } = default!;

        [JsonPropertyName("linkType")]
        public string LinkType { get; set; } = default!;

        [JsonPropertyName("mainCategories")]
        public object MainCategories { get; set; } = default!;

        [JsonPropertyName("media")]
        public object Media { get; set; } = default!;

        [JsonPropertyName("mediaId")]
        public string MediaId { get; set; } = default!;

        [JsonPropertyName("metaDescription")]
        public string MetaDescription { get; set; } = default!;

        [JsonPropertyName("metaTitle")]
        public string MetaTitle { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("navigationSalesChannels")]
        public object NavigationSalesChannels { get; set; } = default!;

        [JsonPropertyName("nestedProducts")]
        public object NestedProducts { get; set; } = default!;

        [JsonPropertyName("parent")]
        public object Parent { get; set; } = default!;

        [JsonPropertyName("parentId")]
        public string ParentId { get; set; } = default!;

        [JsonPropertyName("parentVersionId")]
        public string ParentVersionId { get; set; } = default!;

        [JsonPropertyName("path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("productAssignmentType")]
        public string ProductAssignmentType { get; set; } = default!;

        [JsonPropertyName("productStream")]
        public object ProductStream { get; set; } = default!;

        [JsonPropertyName("productStreamId")]
        public string ProductStreamId { get; set; } = default!;

        [JsonPropertyName("products")]
        public ProductDetails[] Products { get; set; } = default!;

        [JsonPropertyName("seoUrls")]
        public object SeoUrls { get; set; } = default!;

        [JsonPropertyName("serviceSalesChannels")]
        public object ServiceSalesChannels { get; set; } = default!;

        [JsonPropertyName("slotConfig")]
        public object SlotConfig { get; set; } = default!;

        [JsonPropertyName("tags")]
        public object Tags { get; set; } = default!;

        [JsonPropertyName("translated")]
        public object Translated { get; set; } = default!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("versionId")]
        public string VersionId { get; set; } = default!;

        [JsonPropertyName("visible")]
        public bool? Visible { get; set; }
    }
}