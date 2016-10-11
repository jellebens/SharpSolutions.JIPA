using SharpSolutions.JIPA.Models;
using SharpSolutions.JIPA.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SharpSolutions.JIPA.Controls
{
    public sealed partial class TimeTile : UserControl
    {
        public TimeTile()
        {
            this.InitializeComponent();

            this.ViewModel = new TimeViewModel();

            this.DataContext = this.ViewModel.Model;

            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(OnUpdateTimerExpired, TimeSpan.FromMilliseconds(300));
        }

        public TimeViewModel ViewModel { get; private set; }

        private async void OnUpdateTimerExpired(ThreadPoolTimer timer)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, delegate
             {
                 ViewModel.Update();
             });
            
        }
    }
}
