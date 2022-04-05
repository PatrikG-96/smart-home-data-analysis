using System;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.ImportFunctions
{
    public class WideFindMultiTag : ISensor, IExportable
    {
        private static readonly int TYPE_INDEX = 0;
        private static readonly int ID_INDEX = 1;

        private static readonly string TYPE = "REPORT";

        private MqttClient client;

        private Dictionary<string, string> LatestData;

        private List<string> Tags;
        private List<string> Topics;
        private List<byte> QoS;

        public WideFindMultiTag(string brokerAddress)
        {
            Tags = new List<string>();
            Topics = new List<string>();
            QoS = new List<byte>();
            
            client = new MqttClient(brokerAddress);
            LatestData = new Dictionary<string, string>();
        }

        public void AddTopic(string topic)
        {
            Topics.Add(topic);
            QoS.Add(MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public bool Connect(string address, int port)
        {
            string clientId = Guid.NewGuid().ToString();
            try
            {
                client.Connect(clientId);
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
                client.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }

        public void Start()
        {
            if (client.IsConnected)
            {
                client.Subscribe(Topics.ToArray(), QoS.ToArray());
            }
        }

        public void Stop()
        {
            if (client.IsConnected)
            {
                client.Unsubscribe(Topics.ToArray());
            }
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = e.Message.ToString();
            int firstComma = message.IndexOf(",");
            string typeAndId = message.Substring(0, firstComma);
            string[] splitTypeAndId = typeAndId.Split(':');


            if (splitTypeAndId[TYPE_INDEX] == TYPE && Tags.Contains(splitTypeAndId[ID_INDEX]))
            {
                LatestData[splitTypeAndId[ID_INDEX]] = message.Substring(firstComma);
            }

        }
        public Dictionary<string, string> Export()
        {
            // How to handle different tags?

        }
    }
}
