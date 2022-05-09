using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SampleDelete : ICommand
    {
        private readonly List<Sample> _samples;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SampleDelete(List<Sample> samples)
        {
            _samples = samples;
            KeyWord = "delete";
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            foreach (var sample in _samples)
            {
                if (sample.Id == args[0])
                {
                    _samples.Remove(sample);
                    return;
                }
            }
        }
    }
}