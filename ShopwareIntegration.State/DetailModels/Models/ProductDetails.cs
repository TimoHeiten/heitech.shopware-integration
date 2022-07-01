using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;
using heitech.ShopwareIntegration.Models;

namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// represents a DTO with all available properties of the Products Ressource
    /// <para/>
    /// Ressource can be found at https://shopware.stoplight.io/docs/admin-api/c2NoOjE0MzUxMjgy-product
    ///</summary>
    [ModelUri("product")]
    public class ProductDetails : DetailsEntity
    {
        // ctor for serialization purposes
        public ProductDetails()
        { }

        [JsonPropertyName("price")]
        public IReadOnlyList<ProductPrice> Price { get; set; } = default!;

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("availableStock")]
        public long? AvailableStock { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; } = default!;

        [JsonPropertyName("active")]
        public bool? Active { get; set; } = default!;

        [JsonPropertyName("autoIncrement")]
        public long? AutoIncrement { get; set; } = default!;

        [JsonPropertyName("available")]
        public bool? Available { get; set; } = default!;

        [JsonPropertyName("canonicalProduct")]
        public object CanonicalProduct { get; set; } = default!;

        [JsonPropertyName("categories")]
        public object Categories { get; set; } = default!;

        [JsonPropertyName("categoriesRo")]
        public object CategoriesRo { get; set; } = default!;

        [JsonPropertyName("categoryIds")]
        public string[] CategoryIds { get; set; } = default!;

        [JsonPropertyName("categoryTree")]
        public string[] CategoryTree { get; set; } = default!;

        [JsonPropertyName("cheapestPrice")]
        public object CheapestPrice { get; set; } = default!;

        [JsonPropertyName("childCount")]
        public long? ChildCount { get; set; } = default!;

        [JsonPropertyName("children")]
        public object Children { get; set; } = default!;

        [JsonPropertyName("cmsPage")]
        public object CmsPage { get; set; } = default!;

        [JsonPropertyName("cmsPageId")]
        public string CmsPageId { get; set; } = default!;

        [JsonPropertyName("cmsPageVersionId")]
        public string CmsPageVersionId { get; set; } = default!;

        [JsonPropertyName("configuratorGroupConfig")]
        public object ConfiguratorGroupConfig { get; set; } = default!;

        [JsonPropertyName("configuratorSettings")]
        public object ConfiguratorSettings { get; set; } = default!;

        [JsonPropertyName("cover")]
        public object Cover { get; set; } = default!;

        [JsonPropertyName("coverId")]
        public string CoverId { get; set; } = default!;

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; } = default!;

        [JsonPropertyName("crossSellingAssignedProducts")]
        public object CrossSellingAssignedProducts { get; set; } = default!;

        [JsonPropertyName("crossSellings")]
        public object CrossSellings { get; set; } = default!;

        [JsonPropertyName("customFIeldSetSelectionActive")]
        public bool? customFIeldSetSelectionActive { get; set; } = default!;

        [JsonPropertyName("customFieldSets")]
        public object CustomFieldSets { get; set; } = default!;

        [JsonPropertyName("customFields")]
        public object CustomFields { get; set; } = default!;

        [JsonPropertyName("customSearchKeywords")]
        public object[] CustomSearchKeywords { get; set; } = default!;

        [JsonPropertyName("deliveryTime")]
        public string DeliveryTime { get; set; } = default!;

        [JsonPropertyName("deliveryTimeId")]
        public string DeliveryTimeId { get; set; } = default!;

        [JsonPropertyName("displayGroup")]
        public string DisplayGroup { get; set; } = default!;

        [JsonPropertyName("ean")]
        public string Ean { get; set; } = default!;

        [JsonPropertyName("featureSet")]
        public object FeatureSet { get; set; } = default!;

        [JsonPropertyName("feautureSetId")]
        public string FeautureSetId { get; set; } = default!;

        [JsonPropertyName("height")]
        public float? Height { get; set; } = default!;

        [JsonPropertyName("isCloseOut")]
        public bool? IsCloseOut { get; set; } = default!;

        [JsonPropertyName("keywords")]
        public string Keywords { get; set; } = default!;

        [JsonPropertyName("length")]
        public float? Length { get; set; } = default!;

        [JsonPropertyName("mainCategories")]
        public object MainCategories { get; set; } = default!;

        [JsonPropertyName("mainVariantId")]
        public string MainVariantId { get; set; } = default!;

        [JsonPropertyName("manufacturer")]
        public object Manufacturer { get; set; } = default!;

        [JsonPropertyName("manufacturerId")]
        public string ManufacturerId { get; set; } = default!;

        [JsonPropertyName("markAsTopSeller")]
        public bool? MarkAsTopSeller { get; set; } = default!;

        [JsonPropertyName("maxPurchase")]
        public long? MaxPurchase { get; set; } = default!;

        [JsonPropertyName("media")]
        public object Media { get; set; } = default!;

        [JsonPropertyName("metaDescription")]
        public string MetaDescription { get; set; } = default!;

        [JsonPropertyName("metaTitle")]
        public string MetaTitle { get; set; } = default!;

        [JsonPropertyName("minPurchase")]
        public long? MinPurchase { get; set; } = default!;

        [JsonPropertyName("optionIds")]
        public string[] OptionIds { get; set; } = default!;

        [JsonPropertyName("options")]
        public object Options { get; set; } = default!;

        [JsonPropertyName("oderLineItems")]
        public object OderLineItems { get; set; } = default!;

        [JsonPropertyName("packUnit")]
        public string PackUnit { get; set; } = default!;

        [JsonPropertyName("packUnitPlural")]
        public string PackUnitPlural { get; set; } = default!;

        [JsonPropertyName("parent")]
        public object Parent { get; set; } = default!;

        [JsonPropertyName("parentId")]
        public string ParentId { get; set; } = default!;

        [JsonPropertyName("parentVersionId")]
        public string ParentVersionID { get; set; } = default!;

        [JsonPropertyName("prices")]
        public object Prices { get; set; } = default!;

        [JsonPropertyName("productManufacturerVersionId")]
        public string ProductManufacturerVersionId { get; set; } = default!;

        [JsonPropertyName("productMediaVersionID")]
        public string ProductMediaVersionID { get; set; } = default!;

        [JsonPropertyName("productNumber")]
        public string ProductNumber { get; set; } = default!;

        [JsonPropertyName("productReviews")]
        public object ProductReviews { get; set; } = default!;

        [JsonPropertyName("properties")]
        public object Properties { get; set; } = default!;

        [JsonPropertyName("propertyIds")]
        public string[] PropertyIds { get; set; } = default!;

        [JsonPropertyName("purchasePrices")]
        public object PurchasePrices { get; set; } = default!;

        [JsonPropertyName("purchaseSteps")]
        public long? PurchaseSteps { get; set; } = default!;

        [JsonPropertyName("purchaseUnit")]
        public float? PurchaseUnit { get; set; } = default!;

        [JsonPropertyName("ratingAverage")]
        public float? RatingAverage { get; set; } = default!;

        [JsonPropertyName("referenceUnit")]
        public float? ReferenceUnit { get; set; } = default!;

        [JsonPropertyName("releaseDate")]
        public DateTime? ReleaseDate { get; set; } = default!;

        [JsonPropertyName("restockTime")]
        public long? RestockTime { get; set; } = default!;

        [JsonPropertyName("sales")]
        public long? Sales { get; set; } = default!;

        [JsonPropertyName("searchKeywords")]
        public object SearchKeywords { get; set; } = default!;

        [JsonPropertyName("seoUrls")]
        public object SeoUrls { get; set; } = default!;

        [JsonPropertyName("shippingFree")]
        public bool? ShippingFree { get; set; } = default!;

        [JsonPropertyName("slotConfig")]
        public object SlotConfig { get; set; } = default!;

        [JsonPropertyName("streams")]
        public object Streams { get; set; } = default!;

        [JsonPropertyName("tagIds")]
        public string[] TagIds { get; set; } = default!;

        [JsonPropertyName("tags")]
        public object Tags { get; set; } = default!;

        [JsonPropertyName("tax")]
        public object Tax { get; set; } = default!;

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; } = default!;

        [JsonPropertyName("translated")]
        public object Translated { get; set; } = default!;

        [JsonPropertyName("unit")]
        public object Unit { get; set; } = default!;

        [JsonPropertyName("unitId")]
        public string unitId { get; set; } = default!;

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = default!;

        [JsonPropertyName("variantRestrictions")]
        public object VariantRestrictions { get; set; } = default!;

        [JsonPropertyName("variation")]
        public string[] Variation { get; set; } = default!;

        [JsonPropertyName("versionId")]
        public string VersionId { get; set; } = default!;

        [JsonPropertyName("visibilities")]
        public object Visibilities { get; set; } = default!;

        [JsonPropertyName("weight")]
        public float? Weight { get; set; } = default!;

        [JsonPropertyName("width")]
        public float? Width { get; set; } = default!;

        [JsonPropertyName("wishlists")]
        public object Wishlists { get; set; } = default!;
    }
}