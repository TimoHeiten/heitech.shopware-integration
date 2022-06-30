using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Interfaces
{
    ///<summary>
    /// Handles the store for the CRUD operations of the shopware API. Supply the required DataContext for caching and logging strategies.
    ///</summary>
    public interface IStateManager
    {
        Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity;
        Task<IEnumerable<T>> RetrievePage<T>(DataContext context) where T : DetailsEntity;

        Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity;
        Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity;
        Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity;
    }
}