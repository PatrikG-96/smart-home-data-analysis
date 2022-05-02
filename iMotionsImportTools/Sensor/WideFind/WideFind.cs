using System;
using System.Collections.Generic;
using System.Text;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Scheduling;
using Newtonsoft.Json;
using Serilog;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.Sensor.WideFind
{
    
        // TODO: 
        // Allow for multiple tags to be used. Maybe make a handler class where a widefind instance is just a tag intance?
        // Allow for data to be extracted on per tag basis

        public class WideFind : MqttSensor, ITunneler, ISchedulable
        {
            public const string REPORT = "REPORT";
            public const string BEACON = "BEACON";
            public const string LTU_SYSTEM_TOPIC = "ltu-system/#";

            private WideFindJson _latestData;

            public override string Data => _latestData == null ? "null" : _latestData.Message;

            public string Tag { get; set; }
            private readonly List<string> _typeFilters;

            public bool ShouldTunnel { get; set; }
            public event EventHandler<Sample> Transport;
     

            private WideFindJson _scheduledData;

            public bool IsScheduled { get; set; }

            public WideFind(string id, string brokerAddress) : base(id, brokerAddress)
            {
                _typeFilters = new List<string>();
                Tag = "";
                LogName += ":WideFind";
                AddTopic(LTU_SYSTEM_TOPIC);
                _latestData = new WideFindJson();
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


            public override SensorStatus Status()
            {
                var status = new SensorStatus
                {
                    Name = "WideFind",
                    IsConnected = IsConnected,
                    IsStarted = IsStarted,
                    Id = Id,
                    LastMessage = _latestData?.Message,
                    TimeSinceLastMessage = MessageReceivedWatch.ElapsedMilliseconds,
                    TimeAlive = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Optional =
                    {
                        ["Tag"] = Tag
                    }
                };

                return status;
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

                int firstComma = jsonData.Message.IndexOf(',');

                string firstCommaSubstring = jsonData.Message.Substring(0, firstComma);

                string[] typeAndId = firstCommaSubstring.Split(':');

                var type = typeAndId[0];
                var id = typeAndId[1];

               
                if (_typeFilters.Contains(type) && (Tag == id || Tag == "") )
                {
                    
                    MessageReceivedWatch.Restart();
                    
                    lock (_latestData)
                    {
                        _latestData = jsonData;
                    }
                    
                    
                    Console.WriteLine(_latestData.Message);

                   
                    Log.Logger.Debug("{A}:{B} Received data: '{C}'", LogName, Tag, _latestData?.Message ?? "null");
                    

                    if (!ShouldTunnel)
                    {
                        return;
                    }

                    Log.Logger.Debug("{A}:{B} Tunneling message.", LogName, Tag);

                    var ev = Transport;
                    var sample = new VelocitySample();
                    sample.InsertSensorData(this);
                    ev?.Invoke(ev, sample);
                }
                

            }

            public WideFindJson GetData()
            {
            //return IsScheduled ? _scheduledData : _latestData;
                return _latestData?.Copy();
            }



            public void OnScheduledEvent(object sender, SchedulerEventArgs args)
            {
                
                _scheduledData = _latestData;
                Console.WriteLine("WideFind: "+ _latestData);
            }
        }
}

