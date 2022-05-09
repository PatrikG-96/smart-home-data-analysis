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

        private readonly OutputBuilder sensorBuilder;

        public SensorCreate(List<ISensor> sensors)
        {
            KeyWord = "create";
            _sensors = sensors;
            _sensorTypes = new Dictionary<string, Func<string[], ISensor>>();
            Builder = new OutputBuilder();
            sensorBuilder = new OutputBuilder();
            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Error");


            sensorBuilder.AddTitle("title");
            sensorBuilder.AddAttribute("ID");
            sensorBuilder.AddAttribute("Host");
            sensorBuilder.AddAttribute("Tag");
            sensorBuilder.AddAttribute("Devices");
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            Builder.BindValue("title", "Sensor");
            Builder.BindValue("Command", "create");
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

                if (!IsIdUnique(sensor.Id))
                {
                    Builder.BindValue("Status", "Failed");
                    Builder.BindValue("Error", "Sensor ID is already in use");
                    Console.WriteLine(Builder.Build());
                    Builder.Reset();
                    return;
                }

                _sensors.Add(sensor);
                Builder.BindValue("Status", "Success");
                Console.WriteLine(Builder.Build());
                Builder.Reset();

                OutputBuilder.StandardSensorOutput(sensor, sensorBuilder);
                Console.WriteLine(sensorBuilder.Build());
                sensorBuilder.Reset();
            }
        }

        public void AddSensorType(string name, Func<string[], ISensor> constructor)
        {
            _sensorTypes.Add(name, constructor);
        }

        private bool IsIdUnique(string id)
        {
            foreach (var sensor in _sensors)
            {
                if (sensor.Id == id)
                {
                    return false;
                }
            }

            return true;
        }
    }
}