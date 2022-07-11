using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State.Interfaces
{
    ///<summary>
    /// Handles the store for the CRUD operations of the shopware API. Supply the required DataContext for caching and logging strategies.
    ///</summary>
    public interface IStateManager
    {
        ///<summary>
        /// Retrieve a Detail View of the given Resource. Requires a DetailsContext.
        /// <para/>Also to specify what fields you need, you should enrich the DataContext via the SetFilter extension.
        ///</summary>
        Task<T> RetrieveDetails<T>(DataContext context) where T : DetailsEntity;
        ///<summary>
        /// Retrieve a MasterView/Page of the given Resource of T. Requires a PageContext.
        /// <para/>Also to specify what fields you need, and how much items you want to fetch, you should enrich the DataContext via the SetFilter extension.
        ///</summary>
        Task<IEnumerable<T>> RetrievePage<T>(DataContext dataContext) where T : DetailsEntity;

        ///<summary>
        /// Delete a Resource of T. Requires a DeleteContext
        ///</summary>
        Task<T> DeleteAsync<T>(DataContext context) where T : DetailsEntity;
        ///<summary>
        /// Create a new Resource of T. Requires a CreateContext
        ///</summary>
        Task<T> CreateAsync<T>(DataContext context) where T : DetailsEntity;
        ///<summary>
        /// Updates a given Resource of T. Requires an UpdateContext
        ///</summary>
        Task<T> UpdateAsync<T>(DataContext context) where T : DetailsEntity;
    }
}