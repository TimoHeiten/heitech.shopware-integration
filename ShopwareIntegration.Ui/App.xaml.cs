using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ShopwareIntegration.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() 
            => AppDomain.CurrentDomain.UnhandledException += Handler;

        private static void Handler(object sender, UnhandledExceptionEventArgs args)
            => MessageBox.Show($"{args.ExceptionObject}");

        ~App() 
            => AppDomain.CurrentDomain.UnhandledException -= Handler;
    }
}