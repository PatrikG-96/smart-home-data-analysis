using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorAdd : ICommand
    {
        private readonly List<ISensor> _sensors;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SensorAdd(List<ISensor> sensors)
        {
            KeyWord = "add";
            _sensors = sensors;
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            foreach (var sensor in _sensors)
            {
                if (sensor.Id == args[0])
                {
                    controller.AddSensor(sensor);
                    return;
                }
            }
        }
    }
}