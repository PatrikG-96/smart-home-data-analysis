using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace iMotionsImportTools.Sensor
{

    // 130.240.74.55
    // your_mqtt_username
    // your_mqtt_password
    // homeassistant/#

    // TODO: 
    // Better abstraction of a FibaroSensor device.
    // 
    public class FibaroSensor : MqttSensor, ISchedulable, ITunneler
    {
        

        private readonly string _username;
        private readonly string _password;

        private readonly Dictionary<int, FibaroJson> _data;
        private readonly HashSet<int> _deviceIds;

        private Dictionary<int, FibaroJson> _scheduledFrozenData;

        public override string Data
        {
            get
            {
                string s = "";
                foreach (var pair in _data)
                {
                    s += pair.Value.ToString() + ", ";
                }

                return s;
            }
        }

        public bool ShouldTunnel { get; set; }
        public event EventHandler<Sample> Transport;

        public bool IsScheduled { get; set; }

        public FibaroSensor(string id, string brokerAddress, string uname, string pw) : base(id, brokerAddress)
        {
            _password = pw;
            _username = uname;
            _data = new Dictionary<int, FibaroJson>();
            BaseTopic = "homeassistant/sensor/";
            _deviceIds = new HashSet<int>();
        }

        public void AddDevice(int deviceId)
        {
            _data[deviceId] = new FibaroJson();
            _deviceIds.Add(deviceId);
            AddTopic(BaseTopic + deviceId + "/#");
        }

        public void RemoveDevice(int deviceId)
        {
            _data.Remove(deviceId);
            _deviceIds.Remove(deviceId);
            RemoveTopic(BaseTopic + deviceId + "/#");
        }

        public override SensorStatus Status()
        {
            var status = new SensorStatus
            {
                Name = "Fibaro",
                IsConnected = IsConnected,
                IsStarted = IsStarted,
                Id = Id,
                LastMessage = "Hej",
                TimeSinceLastMessage = MessageReceivedWatch.ElapsedMilliseconds,
                TimeAlive = DateTimeOffset.Now.ToUnixTimeMilliseconds() ,
                
            };

            return status;
        }


        public override void OnMessage(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.Default.GetString(e.Message);
            //Console.WriteLine(message + "\n--------------------------------------------------------------------------------");
            try
            {
                var jsonData = JsonConvert.DeserializeObject<FibaroJson>(message);

                if (jsonData == null) return;

                Console.WriteLine($"ID:{jsonData.Id}, Name:{jsonData.DeviceName}, Value:{jsonData.Value}");
                if (jsonData.DeviceName == "U121 Kitchen Temp")
                {
                    Console.WriteLine("Ugly shit");
                    return;
                }
                MessageReceivedWatch.Restart();
                _data[jsonData.Id] = jsonData;



            }
            catch (Exception)
            {
                Console.WriteLine("Probably received an initialization message");
                
            }
        }

        public FibaroJson GetDeviceInfo(int deviceId)
        {
            return _data[deviceId];
        }

        public int[] GetDeviceIds()
        {
            return _deviceIds.ToArray();
        }

        public void OnScheduledEvent(object sender, SchedulerEventArgs args)
        {
            _scheduledFrozenData = new Dictionary<int, FibaroJson>(_data);
        }

        
       
    }
}
