using System;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.Sensor
{
    public abstract class MqttSensor : ISensor
    {

        protected MqttClient Client;

        protected readonly List<string> _topics;
        protected readonly List<byte> _qos;

        public string Id { get; set; }
        public bool IsStarted { get; private set; }

        public bool IsConnected => Client.IsConnected;

        protected MqttSensor(string id, string brokerAddress)
        {
            Client = new MqttClient(brokerAddress);
            Client.MqttMsgPublishReceived += OnMessage;
            _topics = new List<string>();
            _qos = new List<byte>();
            Id = id;
            IsStarted = false;
        }

        public void Start()
        {
            if (IsStarted)
            {
                Console.WriteLine("Sensor already started");
                return;
            }

            if (_qos.Count == 0 || _topics.Count == 0 || _topics.Count != _qos.Count)
            {
                Console.WriteLine("No topics");
                return;
            }
            Client.Subscribe(_topics.ToArray(), _qos.ToArray());
            IsStarted = true;

        }

        public void Stop()
        {
            if (!IsStarted)
            {
                return;
            }
            Client.Unsubscribe(_topics.ToArray());
            IsStarted = false;

        }

        

        public bool Connect()
        {
            string clientId = Guid.NewGuid().ToString();
            try
            {
                Client.Connect(clientId);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                Client.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public void AddTopic(string topic)
        {
            if (_topics.Contains(topic))
            {
                return;
            }
            _topics.Add(topic);
            _qos.Add(MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
        }



        public void RemoveTopic(string topic)
        {
            if (!_topics.Contains(topic))
            {
                return;
            }
            int index = _topics.IndexOf(topic);
            _topics.RemoveAt(index);
            _qos.RemoveAt(index);
        }

        public abstract void OnMessage(object sender, MqttMsgPublishEventArgs e);

    }
}
