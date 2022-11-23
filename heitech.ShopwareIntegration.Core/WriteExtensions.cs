using System.Threading;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Core.Data;
using heitech.ShopwareIntegration.Core.Mutate;

namespace heitech.ShopwareIntegration.Core
{
    public static class WriteExtensions
    {
        /// <summary>
        /// Create a new Entity, as specified in the ModelUri Attribute for the Model.
        /// </summary>
        /// <param name="client">The authenticated IShopwareClient</param>
        /// <param name="payload">The Create model for the desired Entity</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the Entity if successful</returns>
        public static Task<RequestResult<DataEmpty>> CreateAsync<T>(this IShopwareClient client, T payload, CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var request = CreateRequest<T>.Create(client, payload);
            return request.ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Delete an Entity, specified by its id. 
        /// </summary>
        /// <param name="client">The authenticated IShopwareClient</param>
        /// <param name="id">The id for the entity that should be deleted</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>/// <returns>RequestResult´T holding the collection of Entities if successful</returns></returns>
        public static Task<RequestResult<DataEmpty>> DeleteAsync<T>(this IShopwareClient client, string id,  CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var request = DeleteRequest<T>.Create(client, id);
            return request.ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Get a list of entities for the specified type. Be Aware that this might lead to a Shopware Serialization Error if too many Entities exist.
        /// </summary>
        /// <param name="client">The authenticated IShopwareClient</param>
        /// <param name="id">the id for the entity to be updated</param>
        /// <param name="patchedValues">The patched Values (only some writeable properties) for this update</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The Type of the Entity. Including a ModelUriAttribute specifying the url at the Shopware Api</typeparam>
        /// <returns>RequestResult´T holding the collection of Entities if successful</returns>
        public static Task<RequestResult<DataEmpty>> UpdateAsync<T>(this IShopwareClient client, string id, PatchedValues<T> patchedValues,  CancellationToken cancellationToken = default)
            where T : class, IHasShopwareId
        {
            var request = UpdateRequest<T>.Create(client, id, patchedValues);
            return request.ExecuteAsync(cancellationToken);
        }
    }
}