using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SharpSolutions.JIPA.Core.Mqtt
{
    public static class MqttClientFactory
    {
        public const string WillMessage = "offline (unexpected)";
        public const int KeepAlive = 60;
        public static MqttClient CreateSubscriber(string brokerAddress, string clientId)
        {
            MqttClient client = new MqttClient(brokerAddress);
            
            client.ConnectAndSendBirthMessage(clientId);

            return client;
        }

        public static void PublishOnlineMessage(this MqttClient client, string clientId)
        {
            client.Publish(CreateWillTopic(clientId), Encoding.UTF8.GetBytes("online"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
        }

        public static MqttClient ConnectAndSendBirthMessage(this MqttClient client, string clientId) {
            string willTopic = CreateWillTopic(clientId);

            client.Connect(clientId, null, null, true, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true, willTopic, WillMessage, false, KeepAlive);

            client.PublishOnlineMessage(clientId);

            return client;
        }

        public static MqttClient Reconnect(this MqttClient client) {

            client.ConnectAndSendBirthMessage(client.ClientId);
            
            return client;
        }

        public static MqttClient CreatePublisher(string brokerAddress, string clientId) {
            MqttClient client = new MqttClient(brokerAddress);

            client.ConnectAndSendBirthMessage(clientId);

            return client;
        }

        private static string CreateWillTopic(string clientId)
        {
            return string.Format("bir57/system/{0}/status", clientId);
        }
    }
}
