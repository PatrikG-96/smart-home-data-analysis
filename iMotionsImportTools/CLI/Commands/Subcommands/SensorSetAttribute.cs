using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorSetAttribute : ICommand
    {
        private readonly List<ISensor> _sensors;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SensorSetAttribute(List<ISensor> sensors)
        {
            KeyWord = "set-attribute";
            _sensors = sensors;
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            foreach (var sensor in _sensors)
            {
                if (sensor.Id == args[0])
                {
                    Console.WriteLine("Found sensor");

                    if (!Regex.IsMatch(args[1], @"\w+=\w+"))
                    {
                        Console.WriteLine("Failed attribute");
                        return;
                    }

                    if (sensor is WideFind wide)
                    {
                        var keyValue = args[1].Split('=');
                        if (keyValue[0] == "Tag")
                        {
                            Console.WriteLine("Setting tag to " + keyValue[1]);
                            wide.Tag = keyValue[1];
                        }
                    }

                }
            }
        }
    }
}