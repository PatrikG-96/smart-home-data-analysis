using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SampleSubscribe : ICommand
    {
        private readonly List<Sample> _samples;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SampleSubscribe(List<Sample> samples)
        {
            KeyWord = "subscribe";
            _samples = samples;
            Builder = new OutputBuilder();
            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Error");
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            Builder.BindValue("title", "Sample");
            Builder.BindValue("Command", "unsubscribe");
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
                controller.AddSampleSensorSubscription(sampleId, sensorId);
                Builder.BindValue("Status", "Success");
            }
            catch (Exception)
            {
                OutputBuilder.StandardErrorOutput(Builder, "Sensor", "Could not add subscription, either sensor or sample doesn't exist.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
            }
        }

        
    }
}
