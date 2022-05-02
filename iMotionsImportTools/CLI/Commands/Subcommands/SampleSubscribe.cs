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

        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            var sampleId = args[0];
            var sensorId = args[1];

            try
            {
                controller.AddSampleSensorSubscription(sampleId, sensorId);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not add subscription, either sensor or sample doesn't exist");
            }
        }

        
    }
}
