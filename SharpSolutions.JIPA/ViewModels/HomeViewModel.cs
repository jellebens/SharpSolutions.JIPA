using Amqp;
using Newtonsoft.Json;
using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;
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
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.ApplicationModel;
using Windows.System.Threading;
using Windows.UI.Core;

namespace SharpSolutions.JIPA.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        
        private MqttClient _Client;
        private SensorService _SensorService;
        

        public HomeViewModel()
        {
            Time = new TimeModel();
            Temperature = new TemperatureModel();
            Message = new MessageModel();
            PowerConsumption = new ObservableCollection<PowerConsumptionModel>();

            _SensorService = SensorService.Create();

            _Client = MqttClientFactory.CreateSubscriber(Configuration.Current.LocalBus, Configuration.Current.ClientId);
            _Client.MqttMsgPublishReceived += OnClientMessageReceived;
            _Client.Subscribe(new[] { Topics.AllSensors }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(OnKeepAliveTimerElapsed, TimeSpan.FromSeconds(1));
        }

        private async void OnKeepAliveTimerElapsed(ThreadPoolTimer timer)
        {
            if (Dispatcher == null) return; //guard clause

            if (!_Client.IsConnected)
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MessageServiceOffline());

                _Client.Reconnect();
            }
            else {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.Message.UpdateLabel());
            }
        }

        private async void OnClientMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (this.Dispatcher == null) return;

            string msg = Encoding.UTF8.GetString(e.Message);

            MeteringMeasuredEvent evnt = JsonConvert.DeserializeObject<MeteringMeasuredEvent>(msg);

            Sensor s;

            if (!_SensorService.TryGet(evnt.Key, out s)) return;

            //TODO Refactor this
            if (string.Equals(s.Type, "Temperature", StringComparison.CurrentCultureIgnoreCase)) {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {

                    UpdateTemperature(s.Name, float.Parse(evnt.Value));
                });
            }

            if (string.Equals(s.Type, "Power Sensor", StringComparison.CurrentCultureIgnoreCase)) {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    
                    UpdatePower(s.Name, float.Parse(evnt.Value));
                });
            }
            
            
        }

        private void UpdatePower(string name ,float value)
        {


            if (!PowerConsumption.Any(i => i.Name == name))
            {
                PowerConsumptionModel m = new PowerConsumptionModel();
                m.Value = value;
                m.Name = name;
                PowerConsumption.Add(m);
            }
            else {
                PowerConsumptionModel model =  PowerConsumption.Single(i => i.Name == name);
                model.Value = value;
            }
        }

        public TimeModel Time { get; private set; }
        public TemperatureModel Temperature { get; private set; }
        public MessageModel Message { get; private set; }

        public ObservableCollection<PowerConsumptionModel> PowerConsumption { get; set; }

        public CoreDispatcher Dispatcher { get; internal set; }

        public void MessageServiceOffline() {
            this.Message.Label = '\xE871';
        }

        public void UpdateTemperature(string label, float temp)
        {
            Temperature.Label = label;
            Temperature.Temperature = string.Format("{0:#0.0} °C", temp);
        }

        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            Time.Time = now.ToString("HH:mm:ss");
            Time.Date = now.ToString("ddd dd MMM yyy");
        }

    }
}
