using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class ControllerStop: ICommand
    {
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ControllerStop()
        {
            KeyWord = "stop";
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (!controller.IsStarted)
            {
                Console.WriteLine("ControllerCmd not running");
                return;
            }
            controller.StopAll();
        }
    }
}