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
        
        private MqttClient _Client;
        private readonly SensorService _SensorService;
        private readonly LoggingChannel _LoggingChannel;
        private readonly Dictionary<string, float> _SensorValues;
        private int _Reconnecting = 0;

        public void Init()
        {
            Debug.WriteLine("HomeViewModel.Init");
            //To avoid connecting twice
            _Client = MqttClientFactory.CreateSubscriber(Configuration.Current.LocalBus, Configuration.Current.ClientId);
            _Client.MqttMsgPublishReceived += OnClientMessageReceived;
            _Client.ConnectionClosed += OnClientConnectionClosed;
            _Client.Subscribe(new[] { Topics.AllSensors }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        private void OnClientConnectionClosed(object sender, EventArgs e)
        {
            _LoggingChannel.LogMessage("Connection Closed", LoggingLevel.Warning);

            if (_Reconnecting > 0) {
                _LoggingChannel.LogMessage("Allready retrying connecting exiting", LoggingLevel.Information);
                return;
            }

            _Reconnecting++;
            _LoggingChannel.LogMessage("Reconnecting", LoggingLevel.Information);

            try
            {
                int counter = 0;

                while (!_Client.IsConnected)
                {
                    int delayInSeconds = (int)((1d / 2d) * (Math.Pow(2d, counter) - 1d));
                    Task.Delay(delayInSeconds);
                    counter++;
                    _LoggingChannel.LogMessage($"Reconnecting try #{counter} delayed {delayInSeconds}", LoggingLevel.Information);
                    _Client.Reconnect();

                }


            }
            catch (MqttCommunicationException exc)
            {
                _LoggingChannel.LogMessage($"Mqtt Exception {exc.Message}", LoggingLevel.Error);
            }
            finally {
                _Reconnecting--;
            }
        }

        private SemaphoreSlim _Semaphore;
        private long _LastMessage = 0;
        
        public HomeViewModel(): this(new LoggingChannel("HomeViewModelLogger",null))
        {
            
        }

        public HomeViewModel(LoggingChannel logger)
        {

            Debug.WriteLine("HomeViewModel.Ctor");
            _SensorService = SensorService.Create();

            

            _LastMessage = DateTimeOffset.UtcNow.Ticks;

            _LoggingChannel = logger;

            _SensorValues = new Dictionary<string, float>();
        }
       



        private void OnClientMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            _LastMessage = DateTimeOffset.UtcNow.Ticks;
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
