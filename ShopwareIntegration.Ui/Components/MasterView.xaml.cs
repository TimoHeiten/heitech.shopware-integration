using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using heitech.ShopwareIntegration.State.DetailModels;
using ShopwareIntegration.Ui.Components;
using ShopwareIntegration.Ui.Services;
using ShopwareIntegration.Ui.ViewModels;
using ApiContext = heitech.ShopwareIntegration.State.DataContext;

namespace ShopwareIntegration.Ui;

public partial class MasterView : UserControl
{
    /// <summary>
    /// Use Dp property to determine how the dynamic gridview is templated for the
    /// shopware Resource
    /// </summary>
    public MasterView(IEnumerable details)
    {
        InitializeComponent();
        _componentManager = new ComponentManager(Dispatcher);
        DataGrid.ItemsSource = details;
    }
    
    private readonly ComponentManager _componentManager;
    // ReSharper disable once MemberCanBePrivate.Global
    public StateService StateManager { get; init; } = null!;

    private async void Show_Details(object sender, EventArgs args)
    {
        var selected = (DetailViewModelBase)DataGrid.SelectedItem;
        var callback = GetCallbackByType(selected);
        var viewModel = await callback();

        _componentManager.ShowPopup(StateManager, viewModel);
    }

    private Func<Task<DetailViewModelBase>> GetCallbackByType(DetailViewModelBase detailViewModelBase)
    {
        return (detailViewModelBase.Type switch
        {
            DetailTypes.PRODUCTS =>
                async () => await StateManager.GetProductByIdAsync(
                        RessourceId.From(detailViewModelBase.Id),
                        ApiContext.GetDetail<ProductDetails>(detailViewModelBase.Id, detailViewModelBase.Context.PageNo)
                    )
                    .ConfigureAwait(false),

            // TODO adjust others
            DetailTypes.ORDERS => throw new InvalidCastException($"cannot use {detailViewModelBase.Type} here"),
            DetailTypes.CATEGORIES => throw new InvalidCastException($"cannot use {detailViewModelBase.Type} here"),
            DetailTypes.MANUFACTURERS => throw new InvalidCastException($"cannot use {detailViewModelBase.Type} here"),
            _ => throw new InvalidCastException($"cannot use {detailViewModelBase.Type} here")
        });
    }

    private void OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        var propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
        e.Column.Header = propertyDescriptor.DisplayName;

        if (DetailTypes.NotAllowedColumnNames.Any(x =>
                string.Equals(x, propertyDescriptor.DisplayName, StringComparison.InvariantCultureIgnoreCase)))
            e.Cancel = true;
    }
}