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

        public static MqttClient CreateSubscriber(string brokerAddress, string clientId)
        {
            MqttClient client = new MqttClient(brokerAddress);

            string willTopic = CreateWillTopic(clientId);

            client.Connect(clientId, null, null, true, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true, willTopic, WillMessage, false, 60);

            client.PublishOnlineMessage(clientId);

            return client;
        }

        private static void PublishOnlineMessage(this MqttClient client, string clientId)
        {
            client.Publish(CreateWillTopic(clientId), Encoding.UTF8.GetBytes("online"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
        }

        public static MqttClient CreatePublisher(string brokerAddress, string clientId) {
            MqttClient client = new MqttClient(brokerAddress);

            string willTopic = CreateWillTopic(clientId);

            client.Connect(clientId, null, null, true, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true, willTopic, WillMessage, false, 60);

            client.PublishOnlineMessage(clientId);

            return client;
        }

        private static string CreateWillTopic(string clientId)
        {
            return string.Format("bir57/system/{0}/status", clientId);
        }
    }
}
