using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using SharpSolutions.JIPA.ViewModels;
using Windows.System.Threading;
using Windows.UI.Core;
using System.Threading.Tasks;
using SharpSolutions.JIPA.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SharpSolutions.JIPA.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private HomeViewModel _ViewModel;

        public HomePage()
        {
            this.InitializeComponent();

            _ViewModel = App.Container.Resolve<HomeViewModel>();
            
            this.DataContext = _ViewModel;

            _ViewModel.TemperatureChanged += OnTemperatureChanged;
            _ViewModel.TotalPowerConsumptionChanged += OnPowerConsumptionChanged;
        }

        private async void OnPowerConsumptionChanged(object sender, Contracts.TotalPowerConsumtionChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TotalPower.Model.Label = $"Total Power";
                TotalPower.Model.Power = e.Value;
                TotalPower.Model.Unit = e.Unit;
            });
        }

        private async void OnTemperatureChanged(object sender, Contracts.TemperatureChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
             {
                 Temperature.Model.Label = $"{e.Label} ({e.Unit})";
                 Temperature.Model.Temperature = e.Value;
                 Temperature.Model.Unit = e.Unit;
             });
        }
    }
}
