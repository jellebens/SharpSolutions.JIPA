using SharpSolutions.JIPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace SharpSolutions.JIPA.ViewModels
{
    public class HomeViewModel: ViewModelBase
    {
        
        public HomeViewModel()
        {
            Time = new TimeModel();
            Temperature = new TemperatureModel();

            Temperature.Temperature = "29.0 °C";
            Temperature.Pressure = "";
        }

        


    public TimeModel Time { get; private set; }
        public TemperatureModel Temperature { get; private set; }

        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            Time.Time = now.ToString("HH:mm:ss");
            Time.Date = now.ToString("ddd dd MMM yyy");
        }
        
    }
}
