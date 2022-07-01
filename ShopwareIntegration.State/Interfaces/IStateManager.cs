using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Interfaces
{
    ///<summary>
    /// Handles the store for the CRUD operations of the shopware API. Supply the required DataContext for caching and logging strategies.
    ///</summary>
    public interface IStateManager
    {
        ///<summary>
        /// Retrieve Detail View of the given Ressource. Requires a DetailsContext
        ///</summary>
        Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity;
        ///<summary>
        /// Retrieve a MasterView/Page of the given Ressource of T. Requires a PageContext
        ///</summary>
        Task<IEnumerable<T>> RetrievePage<T>(DataContext context) where T : DetailsEntity;

        ///<summary>
        /// Delete a Ressource of T. Requires a DeleteContext
        ///</summary>
        Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity;
        ///<summary>
        /// Create a new Ressource of T . Requires a CreateContext
        ///</summary>
        Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity;
        ///<summary>
        /// Updates a given Ressource of T . Requires an UpdateContext
        ///</summary>
        Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity;
    }
}