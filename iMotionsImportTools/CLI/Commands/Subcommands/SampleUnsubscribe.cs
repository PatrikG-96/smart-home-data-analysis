using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SampleUnsubscribe : ICommand
    {
        private readonly List<Sample> _samples;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SampleUnsubscribe(List<Sample> samples)
        {
            KeyWord = "unsubscribe";
            _samples = samples;
            Builder = new OutputBuilder();
            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Error");
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

            var sampleId = args[0];
            var sensorId = args[1];

            try
            {
                controller.RemoveSampleSensorSubscription(sampleId, sensorId);
                OutputBuilder.StandardSuccessOutput(Builder, "Sample");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
            }
            catch (Exception)
            {
                OutputBuilder.StandardErrorOutput(Builder, "Sensor", $"Could not find sample sensor subscription.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
            }
        }
    }
}