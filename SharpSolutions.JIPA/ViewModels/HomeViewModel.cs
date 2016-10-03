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
using Windows.UI.Core;

namespace SharpSolutions.JIPA.ViewModels
{
    public class HomeViewModel: ViewModelBase
    {
        private string ServiceTopic = Topics.GetJipaSystemTopic() + "/JIPAUi";


        public HomeViewModel()
        {
            Time = new TimeModel();
            Temperature = new TemperatureModel();
            Message = new MessageModel();

            MqttClient client = new MqttClient(Configuration.Current.LocalBus);
            client.MqttMsgPublishReceived += OnClientMessageReceived;

            client.Connect("JIPA UI", null, null, true, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true, ServiceTopic, "offline", false, 0);


            client.Subscribe(new[] { Topics.GetOpenHabTopic() }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Publish(ServiceTopic, Encoding.UTF8.GetBytes("online"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        private async void OnClientMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Message.Toggle = !Message.Toggle;
            
            string msg = Encoding.UTF8.GetString(e.Message);

            MeteringMeasuredEvent evnt = JsonConvert.DeserializeObject<MeteringMeasuredEvent>(msg);

            if (string.Equals(evnt.Key, "jipa_Temperature"))
            {
                string lbl = "Living Room";
                if (Message.Toggle)
                {
                    lbl += ".";
                }
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdateTemperature(float.Parse(evnt.Value), lbl));
            }
        }

        public TimeModel Time { get; private set; }
        public TemperatureModel Temperature { get; private set; }
        public MessageModel Message { get; private set; }
        public CoreDispatcher Dispatcher { get; internal set; }

        public void UpdateTemperature(float temp, string label) {
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
