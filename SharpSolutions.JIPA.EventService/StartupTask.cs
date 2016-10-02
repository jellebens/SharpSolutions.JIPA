using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using System.Diagnostics;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Microsoft.Azure.Devices.Client;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using SharpSolutions.JIPA.Core;
using SharpSolutions.JIPA.Core.Mqtt;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SharpSolutions.JIPA.EventService
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const string JipaSystemTopic = "/jipa/system";

        private BackgroundTaskDeferral _Deferral;
        private DeviceClient _AzureClient;
        private MqttClient _Client;
        private string ServiceTopic = Topics.GetJipaSystemTopic() + "/JIPACloudFowardingService";

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _Deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += OnTaskInstanceCanceled;

            _AzureClient = DeviceClient.Create(Configuration.Default.IotHub, AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(Configuration.Default.DeviceId, Configuration.Default.DeviceKey), TransportType.Amqp);
            
            _Client = new MqttClient(Configuration.Default.LocalBus);
            
            _Client.MqttMsgPublishReceived += OnClientMqttMsgPublishReceived;
            
            _Client.Connect(Configuration.Default.ClientId, null, null, false, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true, ServiceTopic, "offline", false, 0);
            _Client.ConnectionClosed += OnClientConnectionClosed;
            _Client.Publish(ServiceTopic, Encoding.UTF8.GetBytes("online"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

            
            byte[] qos = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };

            _Client.Subscribe(new[] { Topics.GetOpenHabTopic() }, qos);
            string msg = Messages.CreateServiceStartMsg("JIPA Cloud Fowarding Service", Configuration.Default.DeviceId);
            
            _Client.Publish(JipaSystemTopic, Encoding.UTF8.GetBytes(msg));


        }

        private void OnClientConnectionClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("Connection Closed");
        }

        private async void OnClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                Message msg = new Message(e.Message);

                await _AzureClient.SendEventAsync(msg);
            }
            catch (Exception exc)
            {
                string msg = "Cought Exception " + exc.Message;
                Debug.WriteLine(msg);

                _Client.Publish(ServiceTopic + "/error", Encoding.UTF8.GetBytes(msg));
            }
        }

        private void OnTaskInstanceCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            string msg = Messages.CreateServiceStopMsg("JIPA Cloud Fowarding Service", Configuration.Default.DeviceId);
            _Client.Publish(JipaSystemTopic, Encoding.UTF8.GetBytes(msg));
            _Client.Disconnect();
            _AzureClient.Dispose();
            _Deferral.Complete();
        }
    }
}
