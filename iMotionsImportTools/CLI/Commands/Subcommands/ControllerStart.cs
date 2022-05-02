using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class ControllerStart : ICommand
    {
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ControllerStart()
        {
            KeyWord = "start";
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            if (controller.IsStarted)
            {
                Console.WriteLine("ControllerCmd already started");
                return;
            }

            if (!controller.IsConnected)
            {
                Console.WriteLine("Connect the controller first");
                return;
            }
            controller.StartAll();
        }
    }
}