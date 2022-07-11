using System.Threading.Tasks;
using System.Windows.Controls;
using ShopwareIntegration.Ui.Services;
using ShopwareIntegration.Ui.ViewModels;

namespace ShopwareIntegration.Ui.Components.Forms;

public partial class ProductForm : UserControl, IFormValues
{
    private readonly ProductDetailViewModel _viewModel;
    public ProductForm(ProductDetailViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
    }
    
    

    public Task<bool> OnSaveAsync(StateService service)
    {
        return service.Commands.UpdateProduct(_viewModel.Id, _viewModel.Context.PageNo);
    }

    public Task<bool> OnCancelAsync(StateService service)
    {
        return Task.FromResult(true);
    }
}