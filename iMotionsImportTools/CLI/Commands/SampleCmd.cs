using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.CLI.Commands.Subcommands;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;

namespace iMotionsImportTools.CLI.Commands
{
    public class SampleCmd : ICommand
    {
        private List<ICommand> subCommands;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SampleCmd(List<Sample> samples)
        {
            KeyWord = "sample";
            var create = new SampleCreate(samples);
            create.AddSampleType("velocity", new VelocitySample());
            create.AddSampleType("position", new PositionSample());
            create.AddSampleType("fibaroentrance", new EntranceSample());
            subCommands = new List<ICommand>
            {
                create, new SampleAdd(samples), new SampleCreate(samples), new SampleRemove(samples), new SampleSubscribe(samples),
                new SampleUnsubscribe(samples), new SampleLoad(samples)
            };

        }

        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid command");
                return;
            }
            var cmd = FindSubCommand(args[0]);

            if (cmd == null)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            cmd.ExecuteCommand(controller, args.Skip(1).ToArray());
        }

        private ICommand FindSubCommand(string keyword)
        {
            foreach (var cmd in subCommands)
            {
                if (cmd.KeyWord == keyword)
                {

                    return cmd;
                }
            }

            return null;
        }
    }
}