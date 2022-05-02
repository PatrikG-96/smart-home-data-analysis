using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorCreate : ICommand
    {
        private readonly Dictionary<string, Func<string[], ISensor>> _sensorTypes;
        private readonly List<ISensor> _sensors;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SensorCreate(List<ISensor> sensors)
        {
            KeyWord = "create";
            _sensors = sensors;
            _sensorTypes = new Dictionary<string, Func<string[], ISensor>>();
            Builder = new OutputBuilder();
            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Type");
            Builder.AddAttribute("ID");
            Builder.AddAttribute("Broker");
            Builder.AddAttribute("Error");
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            Builder.BindValue("title", "Sensor");
            Builder.BindValue("Command", "Create");
            if (args.Length >= 2)
            {
                
                var type = args[0];
                

                if (!_sensorTypes.ContainsKey(type))
                {
                    Builder.BindValue("Status", "Failed");
                    Builder.BindValue("Error", "Sensor type is invalid");
                    Console.WriteLine(Builder.Build());
                    Builder.Reset();
                    return;
                }
                
                
                var sensor = _sensorTypes[type](args.Skip(1).ToArray());
                _sensors.Add(sensor);
                Builder.BindValue("Status", "Success");
                Builder.BindValue("Type", sensor.GetType().Name);
                Builder.BindValue("ID", sensor.Id);

                if (sensor is MqttSensor mqtt)
                {
                    Builder.BindValue("Broker", mqtt.Broker);
                }
                Console.WriteLine(Builder.Build());
                Builder.Reset();
            }
        }

        public void AddSensorType(string name, Func<string[], ISensor> constructor)
        {
            _sensorTypes.Add(name, constructor);
        }
    }
}