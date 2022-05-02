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
                controller.RemoveSampleSensorSubscription(sampleId, sensorId);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not remove subscription, either sensor or sample doesn't exist");
            }
        }
    }
}