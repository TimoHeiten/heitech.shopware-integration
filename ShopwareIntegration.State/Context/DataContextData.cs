namespace heitech.ShopwareIntegration.State
{
    public static class DataContextData
    {
        ///<summary>
        /// Marks the DataContext as one that is used for a delete request
        ///</summary>
        public const string IS_DELETE = "isDelete";

        public static bool HasDelete(this DataContext ctxt)
        {
            object? delete = null;
            bool? exists = ctxt.AdditionalData?.TryGetValue(IS_DELETE, out delete);

            return exists.HasValue && exists.Value && (bool)delete!;
        }

        public static void AddIsDelete(this DataContext ctxt)
        {
            if (ctxt.AdditionalData is null)
                ctxt.AdditionalData = new Dictionary<string, object>
                {
                    [IS_DELETE] = true
                };
            else 
            {
                if (!ctxt.AdditionalData.ContainsKey(IS_DELETE))
                    ctxt.AdditionalData.Add(IS_DELETE, true);
            }
        }
    }
}