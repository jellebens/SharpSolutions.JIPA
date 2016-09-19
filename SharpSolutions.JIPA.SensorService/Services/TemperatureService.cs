using SharpSolutions.JIPA.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;

namespace SharpSolutions.JIPA.SensorService.Services
{
    public sealed class TemperatureService: IService, IDisposable
    {
        private BMP280 _Sensor;
        private SemaphoreSlim _Semaphore;
        private ThreadPoolTimer _Timer;

        public TemperatureService()
        {
            _Sensor = new BMP280();
            _Semaphore = new SemaphoreSlim(1, 5);
        }
        
        public IAsyncAction Start() {
            ///Crap construction for winrt WME1038
            return AsyncInfo.Run(async delegate (CancellationToken token)
            {
                await _Sensor.Initialize();

                _Semaphore.Release();
                _Timer = ThreadPoolTimer.CreatePeriodicTimer(OnTimerElapsedHandler, TimeSpan.FromSeconds(5));
            });
            
        }

        private async void OnTimerElapsedHandler(ThreadPoolTimer timer)
        {
            _Semaphore.Wait();
            try {
                float temp = await _Sensor.ReadTemperature();

                Debug.WriteLine("{0} °C", temp);
        }
            finally {
                _Semaphore.Release();
            }
            
        }

        public void Dispose()
        {
            _Timer.Cancel();

            _Semaphore.Dispose();
        }
    }
}
