using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorAvailable : ICommand
    {
        private readonly List<ISensor> _sensors;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        private readonly OutputBuilder sensorBuilder;
        public SensorAvailable(List<ISensor> sensors)
        {
            KeyWord = "available";
            _sensors = sensors;
            Builder = new OutputBuilder();
            sensorBuilder = new OutputBuilder();

            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Error");
            Builder.AddAttribute("Sensors");

            sensorBuilder.AddTitle("title");
            sensorBuilder.AddAttribute("ID");
            sensorBuilder.AddAttribute("Host");
            sensorBuilder.AddAttribute("Tag");
            sensorBuilder.AddAttribute("Devices");
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            Builder.BindValue("title", "Sensor");
            Builder.BindValue("Command", "available");


            if (args.Length != 0)
            {
                Builder.BindValue("Status", "Failed");
                Builder.BindValue("Error",$"Expected 0 arguments, received {args.Length}.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
                return;
            }

            Builder.BindValue("Status", "Success");
            Builder.BindValue("Sensors", _sensors.Count.ToString());
            Console.WriteLine(Builder.Build());
            Builder.Reset();

            foreach (var sensor in _sensors)
            {
                OutputBuilder.StandardSensorOutput(sensor, sensorBuilder);
                Console.WriteLine(sensorBuilder.Build());
                sensorBuilder.Reset();
            }
        }
    }
}