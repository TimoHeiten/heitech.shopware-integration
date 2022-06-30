using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;

namespace ShopwareIntegration.State.Interfaces
{
    public interface ICache : IStateManager
    {
        void Add<T>(DataContext context) where T : DetailsEntity;
        void AddMany<T>(DataContext context) where T : DetailsEntity;
    }
}