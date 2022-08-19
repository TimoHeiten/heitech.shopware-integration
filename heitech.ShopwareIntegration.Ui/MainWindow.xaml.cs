using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using heitech.ShopwareIntegration.State.Integration.Configuration;
using MahApps.Metro.Controls;
using ShopwareIntegration.Ui.Components;
using ShopwareIntegration.Ui.Services;
using ShopwareIntegration.Ui.ViewModels;
using ApiDataContext = heitech.ShopwareIntegration.State.DataContext;

namespace ShopwareIntegration.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private int _currentPage = 1;
        private string _currentType = DetailTypes.PRODUCTS ;

        private readonly HttpClientConfiguration _configuration;

        private readonly ErrorPage _error = new();
        private readonly LoadingSpinner _spinner = new();

        public MainWindow()
        {
            InitializeComponent();
            _spinner.Visibility = Visibility.Collapsed;
            // todo build somewhere else with configuration file || load from sql server or similar
            _configuration = new HttpClientConfiguration(
                baseUrl: "http://sw6.wbv24.com/api/",
                clientId: "SWIATKTYADFGUWC2CM53VFKWBG",
                userName: string.Empty,
                clientSecret: "Nk9XQWQzSkRwVnQ2T01LTzJydnM5M3RQTFVJNW1SY3NJM3NTckY"
            );
        }

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems[0]! as ComboBoxItem;
            _currentType = $"{item!.Content}";
            await ChangeMasterViewToAsync().ConfigureAwait(false);
        }
        
        private async void CmdDown_OnClick(object sender, RoutedEventArgs e)
        {
            _currentPage += 1;
            txtNum.Text = $"{_currentPage}";
            await ChangeMasterViewToAsync().ConfigureAwait(false);
        }

        private async void CmdUp_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentPage - 1 <= 0) 
                return;

            _currentPage--;
            txtNum.Text = $"{_currentPage}";

            await ChangeMasterViewToAsync().ConfigureAwait(false);
        }
        
        private async Task ChangeMasterViewToAsync()
        {
            var service = StateService.CreateInstance(_configuration);
            Func<Task<IEnumerable>> getTableData = (_currentType switch
            {
                DetailTypes.PRODUCTS => async () => await service.GetProductsAsync(_currentPage).ConfigureAwait(false),
                DetailTypes.ORDERS => async () => await service.GetOrdersAsync(_currentPage).ConfigureAwait(false),
                DetailTypes.CATEGORIES => async () => await service.GetCategoriesAsync(_currentPage).ConfigureAwait(false),
                DetailTypes.MANUFACTURERS => async () => await service.GetProductManufacturersAsync(_currentPage).ConfigureAwait(false),
                _ => throw new InvalidCastException($"cannot use {_currentType} here")
            });

            ContentArea.Content = _spinner;
            _spinner.Visibility = Visibility.Visible;
            try
            {
                var details = await getTableData().ConfigureAwait(false);
                Dispatcher.Invoke(() =>
                {
                    ContentArea.Content = new MasterView(details)
                    {
                        StateManager = service
                    };
                    _spinner.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception)
            {
                ContentArea.Content = _error;
                service.ForceNewInstance();
            }
        }
    }
}