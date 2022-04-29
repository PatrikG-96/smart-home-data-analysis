using System;
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
            var command = new Command("status", args =>
            {
                _controller.PrintSensorStatuses();
            });
            _interpreter = new Interpreter();
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
                _interpreter.Interpret(splitBySpace[0], splitBySpace);
            }

        }

    }
}