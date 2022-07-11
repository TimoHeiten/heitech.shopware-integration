using System.Threading.Tasks;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using ShopwareIntegration.Ui.Services;
using ShopwareIntegration.Ui.ViewModels;

namespace ShopwareIntegration.Ui.Components;

public sealed class ComponentManager
{
    private readonly Dispatcher _dispatcher;
    public ComponentManager(Dispatcher dispatcher)
        => _dispatcher = dispatcher;

    internal void ShowPopup(StateService state, DetailViewModelBase viewModelBase)
    {
        var parent = new MetroWindow()
        {
            Content = new DetailsPopUp(state, viewModelBase)
            {
                ContentArea =
                {
                    Content = viewModelBase.GenerateViewData()
                }
            }
        };
        parent.ShowDialog();
    }
}