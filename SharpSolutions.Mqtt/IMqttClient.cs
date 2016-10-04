using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.Mqtt
{
    public delegate void MqttMsgPublishEventHandler(object sender, MqttMsgPublishEventArgs e);

    /// <summary>
    /// Delegate that defines event handler for published message
    /// </summary>
    public delegate void MqttMsgPublishedEventHandler(object sender, MqttMsgPublishedEventArgs e);

    /// <summary>
    /// Delagate that defines event handler for subscribed topic
    /// </summary>
    public delegate void MqttMsgSubscribedEventHandler(object sender, MqttMsgSubscribedEventArgs e);

    /// <summary>
    /// Delagate that defines event handler for unsubscribed topic
    /// </summary>
    public delegate void MqttMsgUnsubscribedEventHandler(object sender, MqttMsgUnsubscribedEventArgs e);
    
    public delegate void ConnectionClosedEventHandler(object sender, EventArgs e);

    public interface IMqttClient
    {
        bool CleanSession { get; }
        string ClientId { get; }
        bool IsConnected { get; }
        MqttProtocolVersion ProtocolVersion { get; set; }
        MqttSettings Settings { get; }
        bool WillFlag { get; }
        string WillMessage { get; }
        byte WillQosLevel { get; }
        string WillTopic { get; }

        event ConnectionClosedEventHandler ConnectionClosed;
        event MqttMsgPublishedEventHandler MqttMsgPublished;
        event MqttMsgPublishEventHandler MqttMsgPublishReceived;
        event MqttMsgSubscribedEventHandler MqttMsgSubscribed;
        event MqttMsgUnsubscribedEventHandler MqttMsgUnsubscribed;

        byte Connect(string clientId);
        byte Connect(string clientId, string username, string password);
        byte Connect(string clientId, string username, string password, bool cleanSession, ushort keepAlivePeriod);
        byte Connect(string clientId, string username, string password, bool willRetain, byte willQosLevel, bool willFlag, string willTopic, string willMessage, bool cleanSession, ushort keepAlivePeriod);
        void Disconnect();
        ushort Publish(string topic, byte[] message);
        ushort Publish(string topic, byte[] message, byte qosLevel, bool retain);
        ushort Subscribe(string[] topics, byte[] qosLevels);
        ushort Unsubscribe(string[] topics);
    }
}
