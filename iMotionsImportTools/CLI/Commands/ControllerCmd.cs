using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.CLI.Commands.Subcommands;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands
{
    public class ControllerCmd : ICommand
    {
        private List<ICommand> subCommands;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ControllerCmd()
        {
            KeyWord = "controller";
            subCommands = new List<ICommand>{new ControllerStart(), new ControllerStop(), new ControllerConnect(), new ControllerDisconnect()};
            
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            var cmd = FindSubCommand(args[0]);

            if (cmd == null)
            {
                Console.WriteLine("Subcommand doesnt exist");
                return;
            }

            cmd.ExecuteCommand(controller, args.Skip(1).ToArray());
        }

        private ICommand FindSubCommand(string keyword)
        {
            Console.WriteLine("Looking for subcommand " + keyword);
            foreach (var cmd in subCommands)
            {
                if (cmd.KeyWord == keyword)
                {
                    Console.WriteLine("Found subcommand " + keyword);
                    return cmd;
                }
            }

            return null;
        }
    }
}