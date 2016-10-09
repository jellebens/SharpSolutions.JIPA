using Newtonsoft.Json;
using SharpSolutions.JIPA.Core.Mqtt;
using SharpSolutions.JIPA.Events.Metering;
using SharpSolutions.JIPA.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.Foundation.Diagnostics;
using static SharpSolutions.JIPA.Sensors.PirSensor;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SharpSolutions.JIPA.SensorService.Services
{
    public sealed class MotionService: IService, IDisposable
    {
        private MqttClient _Client;
        private LoggingChannel _LoggingChannel;
        private const string Topic = "bir57/sensors/motion";
        private readonly PirSensor _Sensor;
        private SemaphoreSlim _Semaphore;

        public MotionService(LoggingChannel loggingChannel)
        {
            _Semaphore = new SemaphoreSlim(1);

            _Sensor = new PirSensor(17, SensorType.ActiveHigh);

            _Client = MqttClientFactory.CreatePublisher(Configuration.Default.LocalBus, $"MotionService_{Configuration.Default.ClientId}");

            _LoggingChannel = loggingChannel;
        }

        private void OnMotionDetected(object sender, Windows.Devices.Gpio.GpioPinValueChangedEventArgs e)
        {
            if (!_Semaphore.Wait(0)) return; //if lock is being held exit immediately
            
            try
            {
                MotionDetectedEvent evnt = new MotionDetectedEvent();
                evnt.Key = string.Format("{0}_motion", Configuration.Default.DeviceId);
                evnt.Site = Configuration.Default.Site;
                evnt.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                string payload = JsonConvert.SerializeObject(evnt);

                byte[] msg = Encoding.UTF8.GetBytes(payload);

                if (!_Client.IsConnected)
                {
                    _LoggingChannel.LogMessage("Client is not connected. reconnecting", LoggingLevel.Information);

                    _Client.Reconnect();
                }

                _Client.Publish(Topic, msg, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
            catch (Exception exc)
            {
                string errMsg = "Failed to sent message: " + exc.Message;
                Debug.WriteLine(errMsg);
                _LoggingChannel.LogMessage(errMsg, LoggingLevel.Critical);
            }
            finally {
                _Semaphore.Release();
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("Disposing.");
            _Sensor.Dispose();
            _Client.Disconnect();
        }

        public IAsyncAction Start()
        {
            _LoggingChannel.LogMessage("MotionService.Start()", LoggingLevel.Information);
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            return AsyncInfo.Run(async delegate (CancellationToken token)
            {
                _Sensor.motionDetected += OnMotionDetected;
            });
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        }
    }
}
