namespace ShopwareIntegration.Ui.ViewModels;

public static class DetailTypes
{
    public const string PRODUCTS = "Produkte";
    public const string ORDERS = "Bestellungen";
    public const string CATEGORIES = "Kategorien";
    public const string MANUFACTURERS = "Hersteller";

    public static string[] NotAllowedColumnNames { get; } = new[] { "Id", "ManufacturerId", "Context", "Type" };
}