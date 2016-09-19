using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharpSolutions.JIPA.Events.Metering;
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
        private IBMP280 _Sensor;
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

                TemperatureMeasuredEvent evnt = new TemperatureMeasuredEvent();
                evnt.Site = Configuration.Current.Site;
                evnt.Room = Configuration.Current.Room;
                evnt.Temperature = temp;

                string payload = JsonConvert.SerializeObject(evnt);

                Message msg = new Message(Encoding.UTF8.GetBytes(payload));

                try
                {
                    DeviceClient client = DeviceClient.Create(Configuration.Current.IotHub, AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(Configuration.Current.DeviceId, Configuration.Current.DeviceKey), TransportType.Amqp);
                    await client.SendEventAsync(msg);
                }
                catch (Exception exc) {
                    Debug.WriteLine("-> Failed to sent message: " + exc.Message);
                }
            }finally {
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
