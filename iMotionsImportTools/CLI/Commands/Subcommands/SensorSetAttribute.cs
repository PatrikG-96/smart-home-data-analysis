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
        private OutputBuilder sensorBuilder;

        public SensorSetAttribute(List<ISensor> sensors)
        {
            KeyWord = "set-attribute";
            _sensors = sensors;
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

            if (args.Length != 2)
            {
                OutputBuilder.StandardErrorOutput(Builder, "Sensor", $"Expected 2 arguments, received {args.Length}.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
                return;
            }

            foreach (var sensor in _sensors)
            {
                if (sensor.Id == args[0])
                {

                    if (!Regex.IsMatch(args[1], @"\w+=\w+"))
                    {
                        OutputBuilder.StandardErrorOutput(Builder, "Sensor", $"Attribute format error. Should be: 'AttrName'='AttrValue'. Received: '{args[1]}'");
                        Console.WriteLine(Builder.Build());
                        Builder.Reset();
                        return;
                    }

                    if (sensor is WideFind wide)
                    {
                        var keyValue = args[1].Split('=');
                        if (keyValue[0] == "Tag")
                        {
                            Builder.BindValue("Status", "Success");
                            Console.WriteLine(Builder.Build());
                            Builder.Reset();
                            OutputBuilder.StandardSensorOutput(sensor, sensorBuilder);
                            Console.WriteLine(sensorBuilder.Build());
                            sensorBuilder.Reset();
                            wide.Tag = keyValue[1];
                        }
                    }

                }
            }
        }
    }
}