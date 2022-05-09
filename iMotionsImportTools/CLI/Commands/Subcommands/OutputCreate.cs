using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class OutputCreate: ICommand
    {
        private readonly Dictionary<string, Func<string[], IOutputDevice>> _outputTypes;
        private List<IOutputDevice> outputDevices;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public OutputCreate(List<IOutputDevice> outputs)
        {
            KeyWord = "create";
            outputDevices = outputs;
            _outputTypes = new Dictionary<string, Func<string[], IOutputDevice>>();
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {

            if (args.Length < 2)
            {
                Console.WriteLine("Invalid args");
                return;
            }

            var type = args[0];
            if (!_outputTypes.ContainsKey(type))
            {
                Console.WriteLine("Type doesn't exist");
                return;
            }

            var output = _outputTypes[type](args.Skip(1).ToArray());
            
            outputDevices.Add(output);
        }

        public void AddOutputType(string name, Func<string[], IOutputDevice> constructor)
        {
            _outputTypes.Add(name, constructor);
        }
    }
}