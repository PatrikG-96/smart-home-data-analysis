using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class OutputSet: ICommand
    {
        private readonly Dictionary<string, Func<string[], IOutputDevice>> _outputTypes;
        private List<IOutputDevice> outputDevices;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public OutputSet(List<IOutputDevice> outputs)
        {
            KeyWord = "set";
            outputDevices = outputs;
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            if (controller.IsConnected || controller.IsStarted)
            {
                Console.WriteLine("ControllerCmd is started or connected, can't change output device");
                return;
            }

            var id = args[0];
            var device = FindDevice(id);

            if (device == null)
            {
                Console.WriteLine("Device not found");
                return;
            }

            controller.SetOutputDevice(device);
        }

        private IOutputDevice FindDevice(string id)
        {
            foreach (var output in outputDevices)
            {
                if (output.Id == id)
                {
                    return output;
                }
            }

            return null;
        }


    }
}