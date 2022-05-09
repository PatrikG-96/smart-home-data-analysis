using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class OutputAvailable : ICommand
    {
        private readonly Dictionary<string, Func<string[], IOutputDevice>> _outputTypes;
        private List<IOutputDevice> outputDevices;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public OutputAvailable(List<IOutputDevice> outputs)
        {
            KeyWord = "available";
            outputDevices = outputs;
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {

            if (args.Length > 0)
            {
                Console.WriteLine("Extra arguments ignored");
            }

            foreach (var output in outputDevices)
            {
                Console.WriteLine($"Type:{output.GetType().Name}, Id:{output.Id}");
            }
        }

        
    }
}