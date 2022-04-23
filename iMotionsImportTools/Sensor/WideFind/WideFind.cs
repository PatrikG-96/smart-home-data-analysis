using System;
using System.Collections.Generic;
using System.Text;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Exports;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Scheduling;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.Sensor
{
    
     

        public class WideFind : MqttSensor, ITunneler, ISchedulable, IExportable
        {


            private string _latestData;
            public string Tag { get; set; }
            private readonly List<string> _typeFilters;

            public bool ShouldTunnel { get; set; }
            public event EventHandler<Sample> Transport;

            private string _scheduledData;

            public bool IsScheduled { get; set; }

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

                if (!IsStarted) return;

                var message = Encoding.Default.GetString(e.Message);
                var jsonData = JsonConvert.DeserializeObject<WideFindJson>(message);
                //Console.WriteLine("Raw data: " + _latestData);
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

                
                if (_typeFilters.Contains(type) && (Tag == id || Tag == "") )
                {
                    
                    _latestData = jsonData.message;
                    //Console.WriteLine("Raw data: " + _latestData);
                    if (!ShouldTunnel)
                    {
                        return;
                    }

                    var ev = Transport;
                    ev?.Invoke(ev, VelPosSample.FromString(_latestData));
                }
                

            }

            public ExportData Export()
            {

                string dataToExport = IsScheduled ? _scheduledData : _latestData;
                return WideFindReport.FromString(dataToExport);

            }


            public void OnScheduledEvent(object sender, SchedulerEventArgs args)
            {
                
                _scheduledData = _latestData;
                Console.WriteLine("WideFind: "+ _latestData);
            }
        }
}

