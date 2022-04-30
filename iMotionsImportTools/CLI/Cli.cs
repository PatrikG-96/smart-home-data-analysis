using System;
using System.Linq;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI
{
    public class Cli
    {

        private IMotionsController _controller;
        private Interpreter _interpreter;

        public Cli(IMotionsController controller)
        {
            _controller = controller;
           
            _interpreter = new Interpreter();

            var build = new OutputBuilder();
            build.AddTitle("title");
            build.AddAttribute("ID");
            build.AddAttribute("Connected");
            build.AddAttribute("Started");
            build.AddAttribute("Time Alive");
            build.AddAttribute("Last message");
            build.AddAttribute("Current message");
            var command = new Command("status", build, (ctrlr, args, builder) =>
            {
                
                if (args.Length == 0)
                {
                   
                    var statuses = ctrlr.GetAllSensorStatuses();

                    foreach (var status in statuses)
                    {
                        builder.BindValue("title", status.Name);
                        builder.BindValue("ID", status.Id);
                        builder.BindValue("Connected", status.IsConnected.ToString());
                        builder.BindValue("Started", status.IsStarted.ToString());
                        builder.BindValue("Time Alive", status.TimeAlive.ToString());
                        builder.BindValue("Last message", status.TimeSinceLastMessage.ToString());
                        builder.BindValue("Current message", "REPORT:{tag},0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF");
                        Console.WriteLine(builder.Build());
                    }
                }
            });

            _interpreter.AddCommand(command);

        }

        public void Start()
        {

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();
                if (input == null) continue;
                string[] splitBySpace = input.Split(' ');
                _interpreter.Interpret(splitBySpace[0], splitBySpace.Skip(1).ToArray(), _controller);
            }

        }

    }
}