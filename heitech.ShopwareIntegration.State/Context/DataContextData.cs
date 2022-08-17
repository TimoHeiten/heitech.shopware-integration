using System.Diagnostics.CodeAnalysis;

namespace heitech.ShopwareIntegration.State
{
    public static class DataContextData
    {
        ///<summary>
        /// Marks the DataContext as one that is used for a delete request
        ///</summary>
        private const string IS_DELETE = "isDelete";
        private const string HAS_UPDATE = "hasUpdate";

        internal static bool HasDelete(this DataContext ctxt)
        {
            object? delete = null;
            var exists = ctxt.AdditionalData?.TryGetValue(IS_DELETE, out delete);

            return exists.HasValue && exists.Value && (bool)delete!;
        } 
        
        /// <summary>
        /// checks if an update context object is present. If so return it in the outparam
        /// </summary>
        /// <param name="ctxt"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        internal static bool HasUpdate(this DataContext ctxt, out object? update)
        {
            update = default!;
            var exists = ctxt.AdditionalData?.TryGetValue(HAS_UPDATE, out update);

            return exists.HasValue && exists.Value;
        }

        internal static void AddIsDelete(this DataContext ctxt)
        {
            ctxt.AdditionalData ??= new Dictionary<string, object>()
                {
                    [IS_DELETE] = true
                };

            if (!ctxt.AdditionalData.ContainsKey(IS_DELETE))
                ctxt.AdditionalData.Add(IS_DELETE, true);
        }

        internal static void AddUpdate(this DataContext dataContext, [NotNull] object update)
        {
            dataContext.AdditionalData ??= new Dictionary<string, object>()
            {
                [HAS_UPDATE] = update
            };

            if (!dataContext.AdditionalData.ContainsKey(HAS_UPDATE))
                dataContext.AdditionalData.Add(HAS_UPDATE, update);
        }
    }
}