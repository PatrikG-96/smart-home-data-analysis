using System;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.Sensor
{
    public abstract class MqttSensor : AbstractSensor
    {

        protected MqttClient Client;

        protected readonly List<string> _topics;
        protected readonly List<byte> _qos;

        public bool IsStarted { get; set; }

        protected MqttSensor(string id, string brokerAddress)
        {
            Client = new MqttClient(brokerAddress);
            Client.MqttMsgPublishReceived += OnMessage;
            _topics = new List<string>();
            _qos = new List<byte>();
            Id = id;
            IsStarted = false;
        }

        public override void Start()
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

        public override void Stop()
        {
            if (!IsStarted)
            {
                return;
            }
            Client.Unsubscribe(_topics.ToArray());
            IsStarted = false;

        }

        public override bool Connect()
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

        public override bool Disconnect()
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
