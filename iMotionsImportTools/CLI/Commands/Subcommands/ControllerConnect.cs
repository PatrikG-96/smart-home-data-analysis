using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class ControllerConnect : ICommand
    {
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ControllerConnect()
        {
            KeyWord = "connect";
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {

            if (controller.IsConnected)
            {
                Console.WriteLine("Already connected");
                return;
            }
            controller.ConnectAll();
        }
    }
}