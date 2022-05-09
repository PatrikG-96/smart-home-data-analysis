using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorDelete : ICommand
    {
        private readonly List<ISensor> _sensors;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SensorDelete(List<ISensor> sensors)
        {
            _sensors = sensors;
            KeyWord = "delete";
            Builder = new OutputBuilder();
            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Error");
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            Builder.BindValue("title", "Sensor");
            Builder.BindValue("Command", "delete");
            if (args.Length != 1)
            {
                Builder.BindValue("Status", "Failed");
                Builder.BindValue("Error", $"Expected 1 arguments, received {args.Length}.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
                return;
            }

            foreach (var sensor in _sensors)
            {
                if (sensor.Id == args[0])
                {
                    _sensors.Remove(sensor);
                    Builder.BindValue("Status", "Success");
                    Console.WriteLine(Builder.Build());
                    Builder.Reset();
                    return;
                }
            }
            Builder.BindValue("Status", "Failed");
            Builder.BindValue("Error", $"Did not find sensor with ID '{args[0]}'");
            Console.WriteLine(Builder.Build());
            Builder.Reset();
        }
    }
}