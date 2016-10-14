using Amqp;
using Newtonsoft.Json;
using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;
using SharpSolutions.JIPA.Contracts;
using SharpSolutions.JIPA.Contracts.Data;
using SharpSolutions.JIPA.Core.Mqtt;
using SharpSolutions.JIPA.Events.Metering;
using SharpSolutions.JIPA.Models;
using SharpSolutions.JIPA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.ApplicationModel;
using Windows.Foundation.Diagnostics;
using Windows.System.Threading;
using Windows.UI.Core;

namespace SharpSolutions.JIPA.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        
        private readonly MqttClient _Client;
        private readonly SensorService _SensorService;
        private readonly LoggingChannel _LoggingChannel;
        private readonly Dictionary<string, float> _SensorValues;
        private SemaphoreSlim _Semaphore;
        private int _ReconnectCount = 0;
        
        public HomeViewModel(): this(new LoggingChannel("HomeViewModelLogger",null))
        {

        }

        public HomeViewModel(LoggingChannel logger)
        {
            _SensorService = SensorService.Create();

            _Client = MqttClientFactory.CreateSubscriber(Configuration.Current.LocalBus, Configuration.Current.ClientId);
            _Client.MqttMsgPublishReceived += OnClientMessageReceived;
            _Client.Subscribe(new[] { Topics.AllSensors }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            
            _LoggingChannel = logger;

            _SensorValues = new Dictionary<string, float>();
            _Semaphore = new SemaphoreSlim(1);

            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(OnKeepAliveTimerElapsed, TimeSpan.FromSeconds(60));
        }
        
        private async void OnKeepAliveTimerElapsed(ThreadPoolTimer timer)
        {
            if (!_Semaphore.Wait(0)) return;

            await Task.Run(() =>
            {
                try
                {
                    if (!_Client.IsConnected)
                    {
                        Debug.WriteLine("Reconnecting");
                        _ReconnectCount++;
                        _Client.Reconnect();
                        
                    }
                }
                catch (MqttCommunicationException exc)
                {
                    _LoggingChannel.LogMessage($"Mqtt Exception {exc.Message}", LoggingLevel.Error);
                }
                finally {
                    _Semaphore.Release();
                }
            });
            
        }



        private void OnClientMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);

            MeteringMeasuredEvent evnt = JsonConvert.DeserializeObject<MeteringMeasuredEvent>(msg);

            Sensor s;

            if (!_SensorService.TryGet(evnt.Key, out s)) return;

            //TODO Refactor this
            if (string.Equals(s.Type, "Temperature", StringComparison.CurrentCultureIgnoreCase)) {
                TemperatureChanged?.Invoke(this, new TemperatureChangedEventArgs
                {
                    Key = evnt.Key,
                    Label = s.Name,
                    Unit = s.Unit,
                    Value = float.Parse(evnt.Value)
                });
            }

            if (string.Equals(s.Type, "Power Sensor", StringComparison.CurrentCultureIgnoreCase)) {

                if (!_SensorValues.ContainsKey(evnt.Key)) {
                    _SensorValues.Add(evnt.Key, float.Parse(evnt.Value));
                }
                else {
                    _SensorValues[evnt.Key] = float.Parse(evnt.Value);
                }

                TotalPowerConsumptionChanged?.Invoke(this, new TotalPowerConsumtionChangedEventArgs
                {
                    Unit = s.Unit,
                    Value = _SensorValues.Sum(v => v.Value)
                });
            }

            if (string.Equals(s.Type, "motion", StringComparison.CurrentCultureIgnoreCase)) {
                MotionDetected?.Invoke(this, new MotionDetectedEventArgs {
                    Key = evnt.Key,
                    Label = s.Name
                });
            }
        }
        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;
        public event EventHandler<TotalPowerConsumtionChangedEventArgs> TotalPowerConsumptionChanged;
        public event EventHandler<MotionDetectedEventArgs> MotionDetected;
    }
}
