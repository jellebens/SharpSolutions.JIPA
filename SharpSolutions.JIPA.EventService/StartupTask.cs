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
        
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _Deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += OnTaskInstanceCanceled;

            _AzureClient = DeviceClient.Create(Configuration.Default.IotHub, AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(Configuration.Default.DeviceId, Configuration.Default.DeviceKey), TransportType.Amqp);
            
            _Client = new MqttClient(Configuration.Default.LocalBus);
            _Client.MqttMsgPublishReceived += OnClientMqttMsgPublishReceived;
            _Client.Connect(Configuration.Default.ClientId);
            

            
            byte[] qos = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };

            _Client.Subscribe(new[] { Topics.GetOpenHabTopic() }, qos);

            string msg = Messages.CreateServiceStartMsg("JIPA Cloud Fowarding Service", Configuration.Default.DeviceId);
            
            _Client.Publish(JipaSystemTopic, Encoding.UTF8.GetBytes(msg));


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
                Debug.WriteLine("Cought Exception " + exc.Message);
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
