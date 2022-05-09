using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class ControllerDisconnect : ICommand
    {
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ControllerDisconnect()
        {
            KeyWord = "disconnect";
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {

            if (!controller.IsConnected)
            {
                Console.WriteLine("Already disconnected");
                return;
            }
            controller.DisconnectAll();
        }
    }
}
