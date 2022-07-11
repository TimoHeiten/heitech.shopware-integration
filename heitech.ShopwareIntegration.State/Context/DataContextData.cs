using System.Diagnostics.CodeAnalysis;

namespace heitech.ShopwareIntegration.State
{
    public static class DataContextData
    {
        ///<summary>
        /// Marks the DataContext as one that is used for a delete request
        ///</summary>
        public const string IS_DELETE = "isDelete";

        public const string HAS_UPDATE = "hasUpdate";
        public static bool HasDelete(this DataContext ctxt)
        {
            object? delete = null;
            bool? exists = ctxt.AdditionalData?.TryGetValue(IS_DELETE, out delete);

            return exists.HasValue && exists.Value && (bool)delete!;
        } 
        
        /// <summary>
        /// checks if an update context object is present. If so return it in the outparam
        /// </summary>
        /// <param name="ctxt"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public static bool HasUpdate(this DataContext ctxt, out object? update)
        {
            update = default!;
            bool? exists = ctxt.AdditionalData?.TryGetValue(HAS_UPDATE, out update);

            return exists.HasValue && exists.Value;
        }

        public static void AddIsDelete(this DataContext ctxt)
        {
            ctxt.AdditionalData ??= new Dictionary<string, object>()
                {
                    [IS_DELETE] = true
                };
            
            if (!ctxt.AdditionalData.ContainsKey(IS_DELETE))
                ctxt.AdditionalData.Add(IS_DELETE, true);
        }

        public static void AddUpdate(this DataContext dataContext, [NotNull] object update)
        {
            dataContext.AdditionalData ??= new Dictionary<string, object>()
            {
                [HAS_UPDATE] = update
            };

            if (dataContext.AdditionalData is not null)
            {
                dataContext.AdditionalData.Add(HAS_UPDATE, update);
            }
        }
    }
}