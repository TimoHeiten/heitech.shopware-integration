using System;
using System.Windows.Controls;

namespace ShopwareIntegration.Ui.Components;

public partial class ErrorPage : UserControl
{
    public ErrorPage()
    {
        InitializeComponent();
    }

    public Exception Exception { get; set; } = default!;
}