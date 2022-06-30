namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Marker class for the Retrieve operation
    ///</summary>
    public sealed class DetailRequestModel<T> : DetailsEntity
        where T : DetailsEntity
    {
        public T Result { get; set; } = default!;

    }
}