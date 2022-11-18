using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Logging
{
    internal static class LoggingData
    {
        private const string IS_ENTRY = "isEntry";
        private const string IS_ERROR = "error";
        public static DataContext PrepareLogging(this DataContext context, bool isEntry = true)
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

        public static DataContext PrepareError(this DataContext context, Exception ex)
        {
            context.AdditionalData??= new Dictionary<string, object>();
            context.AdditionalData.Add(IS_ERROR, ex);
            return context;
        }

        public static bool ReadIsEntry<T>(this DataContext ctxt)
            where T : DetailsEntity
        {
            var exists = ctxt.AdditionalData!.TryGetValue(IS_ENTRY, out object? isEntry);
            return exists && (bool)isEntry!;
        }

        public static Exception? GetError(this DataContext context)
        {
            var exists = context.AdditionalData!.TryGetValue(IS_ERROR, out var error);
            return exists && error is Exception e ? e : null;
        }
    }
}