using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.CLI.Commands
{
    public class OutputDelete : ICommand
    {
        private readonly Dictionary<string, Func<string[], IOutputDevice>> _outputTypes;
        private List<IOutputDevice> outputDevices;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public OutputDelete(List<IOutputDevice> outputs)
        {
            KeyWord = "delete";
            outputDevices = outputs;
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }
            var id = args[0];
            if (controller.GetActiveOutputDeviceId() == id)
            {
                Console.WriteLine("ControllerCmd is using this device, can't remove.");
                return;
            }


            foreach (var output in outputDevices)
            {
                if (output.Id == id)
                {
                    outputDevices.Remove(output);
                    return;
                }
            }


        }

      
    }
}