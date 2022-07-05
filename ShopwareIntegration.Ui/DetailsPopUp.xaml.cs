using System.Windows.Controls;
using ShopwareIntegration.Ui.ViewModels;

namespace ShopwareIntegration.Ui;

public partial class DetailsPopUp : UserControl
{
    public DetailsPopUp()
    {
        InitializeComponent();
    }

    public ProductDetailViewModel ViewModel { get; set; } = null!;
}