namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// placeholder/marker for PageRequest UseCase
    ///</summary>
    public class PageRequestModel<T> : DetailsEntity
        where T : DetailsEntity
    {
        public IEnumerable<T> Result { get; set; } = Array.Empty<T>();

    }
}