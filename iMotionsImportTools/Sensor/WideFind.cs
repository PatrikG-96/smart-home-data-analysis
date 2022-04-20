using System;
using System.Collections.Generic;
using System.Text;
using iMotionsImportTools.Exports;
using iMotionsImportTools.ImportFunctions;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.Sensor
{
    
        public class WideFindJSON
        {
            internal static readonly List<string> FieldNames = new List<string>()
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

        public class WideFind : MqttSensor, IExportable
        {


            private string _latestData;
            public string Tag { get; set;}
            private readonly List<string> _typeFilters;

            public WideFind(string id, string brokerAddress) : base(id, brokerAddress)
            {
                _typeFilters = new List<string>();
                Tag = "";
            }

            public void AddType(string typeName)
            {
                if (_typeFilters.Contains(typeName))
                {
                    return;
                }
                _typeFilters.Add(typeName);
            }
            public bool RemoveType(string typeName)
            { 
                return _typeFilters.Remove(typeName);
            }


            public override void OnMessage(object sender, MqttMsgPublishEventArgs e)
            {
                var message = Encoding.Default.GetString(e.Message);
                var jsonData = JsonConvert.DeserializeObject<WideFindJSON>(message);

                if (jsonData == null)
                {
                    return;
                }

                // Check if source field in widefind json is useable instead of manipulating string

                int firstComma = jsonData.message.IndexOf(',');

                string firstCommaSubstring = jsonData.message.Substring(0, firstComma);

                string[] typeAndId = firstCommaSubstring.Split(':');

                var type = typeAndId[0];
                var id = typeAndId[1];

                Console.WriteLine("type: " + type);
                Console.WriteLine("id: " + id);
                Console.WriteLine("tag: " + Tag);
            if (_typeFilters.Contains(type) && (Tag == id || Tag == ""))
                {
                    Console.WriteLine("inhere");
                    _latestData = jsonData.message;
                }
                

            }

            public ExportData Export()
            {
                Console.WriteLine(_latestData);
                return WideFindReportData.FromString(_latestData);

            }
            
    }
}

