using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using iMotionsImportTools.CLI.Commands.Subcommands;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.CLI.Commands
{
    public class OutputCmd : ICommand
    {
        private List<ICommand> subCommands;

        
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public OutputCmd(List<IOutputDevice> outputs)
        {
            KeyWord = "output";
            
            var create = new OutputCreate(outputs);
            create.AddOutputType("stdout", (s) =>
            {
                if (s.Length == 0)
                {
                    Console.WriteLine("Invalid arguments");
                    return null;
                }
                if (s.Length > 1)
                {
                    Console.WriteLine("Extra arguments found, ignoring them");
                }

                var output = new Stdout();
                output.Id = s[0];
                return output;
            });
            create.AddOutputType("file", (s) =>
            {
                if (s.Length == 0)
                {
                    Console.WriteLine("Invalid arguments");
                    return null;
                }
                if (s.Length > 2)
                {
                    Console.WriteLine("Extra arguments found, ignoring them");
                }

                var name = s[1];
                var output =  new FileOutput(name);
                output.Id = s[0];
                return output;
            });
            create.AddOutputType("remote-server", (s) =>
            {
                if (s.Length == 0)
                {
                    Console.WriteLine("Invalid arguments");
                    return null;
                }
                if (s.Length > 3)
                {
                    Console.WriteLine("Extra arguments found, ignoring them");
                }

                var addr = s[1];
                int port = Convert.ToInt32(s[2]);
                var output = new AsyncTcpClient();
                output.Connect(new ServerInfo(addr, port), CancellationToken.None).Wait();
                output.Id = s[0];
                return output;
            });
            subCommands = new List<ICommand>{create, new OutputAvailable(outputs), new OutputDelete(outputs), new OutputSet(outputs)};
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
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