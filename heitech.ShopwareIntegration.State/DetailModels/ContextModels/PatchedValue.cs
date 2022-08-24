namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Specify the patched Values for the UpdateOperation. Use an anonymous or specialized object 
    ///</summary>
    public sealed class PatchedValue : DetailsEntity
    {
        public object Values { get; }
        public DetailsEntity Source { get; private init; }
        private PatchedValue(DetailsEntity source, object patchedValues) => (Id, Source, Values) = (source.Id, source, patchedValues);

        /// <summary>
        /// Ctor shortHand with generic Type
        /// </summary>
        /// <param name="entity">An instance of any DetailsEntity Type you want to update</param>
        /// <param name="patchedValues">An object that specifies the fields that should be updated (make sure those exist, have a valid range and are allowed to be updated)</param>
        /// <typeparam name="T">The DetailsEntity Type you want to update</typeparam>
        /// <returns></returns>
        public static PatchedValue From<T>(T entity, object patchedValues) where T : DetailsEntity
            => new(entity, patchedValues);
    }
}