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
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            
            if (args.Length >= 2)
            {
                Console.WriteLine("here");
                var type = args[0];
                

                if (!_sensorTypes.ContainsKey(type))
                {
                    Console.WriteLine("Type doesn't exist");
                    return;
                }
                
                var sensor = _sensorTypes[type](args.Skip(1).ToArray());
                _sensors.Add(sensor);
            }
        }

        public void AddSensorType(string name, Func<string[], ISensor> constructor)
        {
            _sensorTypes.Add(name, constructor);
        }
    }
}