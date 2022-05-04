using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.CLI.Commands.Subcommands;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands
{
    public class ConfigCmd : ICommand
    {
        private List<ICommand> subCommands;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ConfigCmd(OutputLoad output, SensorLoad sensorLoad, SampleLoad sampleLoad)
        {
            KeyWord = "config";
            subCommands.Add(new ConfigLoad(output, sensorLoad, sampleLoad));
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Error");
                return;
            }

            var cmd = FindSubCommand(args[0]);

            if (cmd == null)
            {
                Console.WriteLine("invalid command");
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