using SharpSolutions.JIPA.Models;
using SharpSolutions.JIPA.Service;
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
        private TemperatureService _TemperatureService;

        public HomeViewModel()
        {
            Time = new TimeModel();
            Temperature = new TemperatureModel();

            Temperature.Temperature = "";
            Temperature.Pressure = "";
        }

        public HomeViewModel(TemperatureService temperatureService) : this()
        {
            _TemperatureService = temperatureService;
            
            UpdateTime();
        }


    public TimeModel Time { get; private set; }
        public TemperatureModel Temperature { get; private set; }

        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            Time.Time = now.ToString("HH:mm:ss");
            Time.Date = now.ToString("ddd dd MMM yyy");
        }

        public async Task Init()
        {
            await _TemperatureService.InitSensor();
        }

        public async void UpdateTemperature() {
            float temparature = await _TemperatureService.ReadTemperature();
            Temperature.Temperature = string.Format("{0:#0.0} °C", temparature);
        }

        public async void UpdatePressure()
        {
            float pressure = await _TemperatureService.ReadPreassure();
            Temperature.Pressure = string.Format("{0:#0.00} kPa", pressure /1000.00);
        }
    }
}
