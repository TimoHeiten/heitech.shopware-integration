using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Logging
{
    public static class LoggingData
    {
        public const string IS_ENTRY = "isEntry";
        public const string IS_ERROR = "hasError";
        public static DataContext PrepareLogging<T>(this DataContext context, bool isEntry = true)
           where T : DetailsEntity
        {
            if (context.AdditionalData is null)
                context.AdditionalData = new Dictionary<string, object> { [IS_ENTRY] = isEntry };
            else
            {
                if (!context.AdditionalData.ContainsKey(IS_ENTRY))   
                    context.AdditionalData.Add(IS_ENTRY, isEntry);
                else
                    context.AdditionalData[IS_ENTRY] = isEntry;
            }

            return context;
        }

        public static bool ReadIsEntry<T>(this DataContext ctxt)
            where T : DetailsEntity
        {
            if (ctxt.AdditionalData is null) return false;

            bool exists = ctxt.AdditionalData.TryGetValue(IS_ENTRY, out object? isEntry);
            return exists && (bool)isEntry!;
        }
    }
}