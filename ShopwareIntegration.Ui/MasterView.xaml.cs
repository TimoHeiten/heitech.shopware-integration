using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using heitech.ShopwareIntegration.ProductUseCases;
using heitech.ShopwareIntegration.State.Api;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;
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
    public MasterView()
    {
        InitializeComponent();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public StateService StateManager { get; init; } = null!;

    private async void Row_DoubleClick(object sender, MouseEventArgs args)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (StateManager is null)
            return;

        if (sender is not DataGridRow { Item: ProductMasterViewModel masterViewModel })
        {
            throw new Exception("not the correct event brah");
        }

        return;
        
        // var filter = new {
        //     limit = 100,
        //     page = 1,
        //     includes = new IncludesFields.Product("name", "price", "id", "active", "availableStock", "description", "ean", "stock", "manufacturerId"),
        // };
        // var productContext = ApiContext.GetDetail<ProductDetails>(masterViewModel.Id, masterViewModel.DataContext.PageNo);
        // productContext.SetFilter(filter);
        // var manufacturerContext = ApiContext.GetDetail<ProductManufacturerDetails>(masterViewModel.ManufacturerId, masterViewModel.DataContext.PageNo);
        //     
        // var productTask = StateManager.RetrieveDetails<ProductDetails>(productContext);
        // var manufacturerTask = StateManager.RetrieveDetails<ProductManufacturerDetails>(manufacturerContext);
        //
        // await Task.WhenAll(productTask, manufacturerTask);
        //
        // var viewModel = new ProductDetailViewModel(productTask.Result, manufacturerTask.Result);
        //
        // var popup = new DetailsPopUp
        // {
        //     ViewModel = viewModel
        // };
        // var handle = new Window
        // {
        //     Content = popup
        // };
        // handle.ShowDialog();
    }

    private static readonly string[] _notAllowedColumns = new[] { "Id", "ManufacturerId", "DataContext" }; 
    private void OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        var propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
        e.Column.Header = propertyDescriptor.DisplayName;
        
        if (_notAllowedColumns.Any(x=> propertyDescriptor.DisplayName == x))
            e.Cancel = true;
    }
}