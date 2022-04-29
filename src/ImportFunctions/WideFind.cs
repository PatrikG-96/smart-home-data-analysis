using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iMotionsImportTools.ImportFunctions
{
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
        public class WideFindJSON
        {
            internal static readonly List<string> ValueNames = new List<string>()
            {
                "version", "posX", "posY", "posZ", "velX", "velY",
                "velZ", "battery", "rssi", "timealive"
            };
            public string host { get; set; }
            public string message { get; set; }
            public string source { get; set; }
            public string time { get; set; }
            public string type { get; set; }
        }

        public class WideFind : ISensor, IExportable
        {
            private const int TypeIndex = 0;
            private const int IdIndex = 1;
            private const string Type = "REPORT";
            

            private readonly MqttClient _client;

            private string _latestData;
            private string _tag;
            private readonly List<string> _topics;
            private readonly List<byte> _qos;

            public WideFind(string brokerAddress)
            {
                _tag = "";
                _latestData = "";

                _topics = new List<string>();
                _qos = new List<byte>();

                _client = new MqttClient(brokerAddress);
            }

            public void AddTopic(string topic)
            {
                _topics.Add(topic);
                _qos.Add(MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
            }

            public void AddTag(string tag)
            {
                _tag=tag;
            }

            public void RemoveTag()
            {
                _tag = "";
            }

            public bool Connect(string address, int port)
            {
                string clientId = Guid.NewGuid().ToString();
                try
                {
                    _client.Connect(clientId);
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
                    _client.Disconnect();
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
                if (_client.IsConnected)
                {
                    _client.Subscribe(_topics.ToArray(), _qos.ToArray());
                }
            }

            public void Stop()
            {
                if (_client.IsConnected)
                {
                    _client.Unsubscribe(_topics.ToArray());
                }
            }

            private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
            {
                // Parse the WideFind message to JSON
                var json = JsonConvert.DeserializeObject<WideFindJSON>(e.Message.ToString());

                // Source field in JSON? Look into it.
                string message = json.message;
                int firstComma = message.IndexOf(",");
                string typeAndId = message.Substring(0, firstComma);
                string[] splitTypeAndId = typeAndId.Split(':');


                if (splitTypeAndId[TypeIndex] == Type && _tag == splitTypeAndId[IdIndex])
                {
                    _latestData = message.Substring(firstComma);
                }

            }
            public Dictionary<string, string> Export()
            {
                string[] splitParameters = _latestData.Split(',');
                var result = new Dictionary<string, string>();

                for (var i = 0; i < splitParameters.Length; i++)
                {
                    result[WideFindJSON.ValueNames[i]] = splitParameters[i];
                }

                return result;
            }
        }
    }

}
