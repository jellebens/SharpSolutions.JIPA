using SharpSolutions.JIPA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class PowerConsumtionTile : UserControl
    {
        //public static DependencyProperty MinProperty = 
        //    DependencyProperty.Register("Min", typeof(int), typeof(PowerConsumtionTile), new PropertyMetadata(false));
        
        

        //public static DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(int), typeof(PowerConsumtionTile), new PropertyMetadata(false));

        public PowerConsumtionTile()
        {
            this.InitializeComponent();
            this.Model = new PowerConsumptionModel();
        }

        public PowerConsumptionModel Model { get; private set; }


        //public int Min
        //{
        //    get { return (int)base.GetValue(MinProperty); }
        //    set { base.SetValue(MinProperty, value); }
        //}

        //public int Max
        //{
        //    get { return (int)base.GetValue(MaxProperty); }
        //    set { base.SetValue(MaxProperty, value); }
        //}



        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(PowerConsumtionTile), new PropertyMetadata(0));



        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(PowerConsumtionTile), new PropertyMetadata(0));



    }
}
