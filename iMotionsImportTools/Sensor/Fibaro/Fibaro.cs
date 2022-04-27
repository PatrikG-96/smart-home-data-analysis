﻿using System;
using System.Collections.Generic;
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
    // Better abstraction of a Fibaro device.
    // 
    public class Fibaro : MqttSensor, ISchedulable, ITunneler
    {
        private readonly string _username;
        private readonly string _password;

        private readonly Dictionary<int, string> _data;
        private Dictionary<int, string> _scheduledFrozenData;

        public bool ShouldTunnel { get; set; }
        public event EventHandler<Sample> Transport;

        public bool IsScheduled { get; set; }

        public Fibaro(string id, string brokerAddress, string uname, string pw) : base(id, brokerAddress)
        {
            _password = pw;
            _username = uname;
            _data = new Dictionary<int, string>();
        }


        public new bool Connect()
        {
            var clientId = Guid.NewGuid().ToString();
            try
            {
                Client.Connect(clientId, username: _username, password:_password);
                Console.WriteLine("connected");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
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
                _data[jsonData.Id] = jsonData.Value;



            }
            catch (Exception)
            {
                Console.WriteLine("Probably received an initialization message");
                
            }
        }

   

        

        public void OnScheduledEvent(object sender, SchedulerEventArgs args)
        {
            _scheduledFrozenData = new Dictionary<int, string>(_data);
        }

        
       
    }
}
