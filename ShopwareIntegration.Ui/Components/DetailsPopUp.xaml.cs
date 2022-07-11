using System;
using System.Windows;
using System.Windows.Controls;
using ShopwareIntegration.Ui.Components.Forms;
using ShopwareIntegration.Ui.Services;
using ShopwareIntegration.Ui.ViewModels;

namespace ShopwareIntegration.Ui;

public partial class DetailsPopUp : UserControl
{
    private readonly StateService _state;
    public DetailViewModelBase ViewModel { get; init; } = null!;

    public DetailsPopUp(StateService state, DetailViewModelBase detailsViewModel)
    {
        InitializeComponent();
        _state = state;
        ViewModel = detailsViewModel;
        HideBanners();
    }

    private void HideBanners()
    {
        bannerSuccess.Visibility = Visibility.Collapsed;
        bannerFailed.Visibility = Visibility.Collapsed;
    }

    private async void Save_OnClick(object sender, RoutedEventArgs e)
    {
        HideBanners();
        if (ContentArea.Content is not IFormValues values)
            throw new Exception($"every Form needs to implement the IFormValues interface! But {ContentArea.Content.GetType()} does not");

        var success = await values.OnSaveAsync(_state);
        if (success)
            bannerSuccess.Visibility = Visibility.Visible;
        else
            bannerFailed.Visibility = Visibility.Visible;
    }

    private async void Abbrechen_OnClick(object sender, RoutedEventArgs e)
    {
        HideBanners();
        if (ContentArea.Content is not IFormValues values)
            throw new Exception($"every Form needs to implement the IFormValues interface! But {ContentArea.Content.GetType()} does not");

        await values.OnCancelAsync(_state);
        
        (Parent as Window)!.Close();
    }
}