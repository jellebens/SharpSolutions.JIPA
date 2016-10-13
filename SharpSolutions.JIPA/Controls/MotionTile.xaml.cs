using SharpSolutions.JIPA.Models;
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
    public sealed partial class MotionTile : UserControl
    {
        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(PowerConsumtionTile), new PropertyMetadata(0));
        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(PowerConsumtionTile), new PropertyMetadata(0));



        public MotionTile()
        {
            this.InitializeComponent();
            this.Model = new MotionModel();
            this.DataContext = this.Model;

            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(OnDecreaseTimerElapsedHandler, TimeSpan.FromMilliseconds(1000));
        }

        private async void OnDecreaseTimerElapsedHandler(ThreadPoolTimer timer)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Model.Decrease();
            });
        }

        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        

        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public MotionModel Model { get; private set; }

        

    }
}
