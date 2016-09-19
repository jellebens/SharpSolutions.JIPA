using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using System.Diagnostics;
using SharpSolutions.JIPA.Sensors;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SharpSolutions.JIPA.SensorService
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _Deferral;
        BMP280 _Sensor;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _Deferral = taskInstance.GetDeferral();


            Configuration config = Configuration.Load();

            Debug.WriteLine(config.DeviceId);

            _Sensor = new BMP280();

            await _Sensor.Initialize();

            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(OnSensorTimerElapsedHandler, TimeSpan.FromSeconds(5));

        }

        private async void OnSensorTimerElapsedHandler(ThreadPoolTimer timer)
        {
            float temp = await _Sensor.ReadTemperature();
            Debug.WriteLine(temp);
        }
    }
}
