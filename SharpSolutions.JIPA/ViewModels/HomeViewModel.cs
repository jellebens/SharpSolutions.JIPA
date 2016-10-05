using Amqp;
using Newtonsoft.Json;
using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;
using SharpSolutions.JIPA.Core.Mqtt;
using SharpSolutions.JIPA.Events.Metering;
using SharpSolutions.JIPA.Models;
using System;
using System.Collections.Generic;
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
        private string ServiceTopic = Topics.GetJipaSystemTopic() + "/JIPAUi";
        private MqttClient _Client;

        public HomeViewModel()
        {
            Time = new TimeModel();
            Temperature = new TemperatureModel();
            Message = new MessageModel();

            _Client = MqttClientFactory.CreateSubscriber(Configuration.Current.LocalBus, Configuration.Current.ClientId);
            _Client.MqttMsgPublishReceived += OnClientMessageReceived;


            _Client.Subscribe(new[] { Topics.Temperature }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(OnKeepAliveTimerElapsed, TimeSpan.FromSeconds(30));
        }

        private async void OnKeepAliveTimerElapsed(ThreadPoolTimer timer)
        {
            if (Dispatcher == null) return; //guard clause

            if (!_Client.IsConnected) {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MessageServiceOffline());
                
                _Client.Reconnect();
            }
        }

        private async void OnClientMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);

            MeteringMeasuredEvent evnt = JsonConvert.DeserializeObject<MeteringMeasuredEvent>(msg);

            string lbl = "Living Room";
            
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                this.Message.Label = '\xE870';
                UpdateTemperature(float.Parse(evnt.Value), lbl);
            });
        }

        public TimeModel Time { get; private set; }
        public TemperatureModel Temperature { get; private set; }
        public MessageModel Message { get; private set; }
        public CoreDispatcher Dispatcher { get; internal set; }

        public void MessageServiceOffline() {
            this.Message.Label = '\xE871';
        }

        public void UpdateTemperature(float temp, string label)
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
