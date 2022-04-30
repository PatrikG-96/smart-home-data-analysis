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
           
            _interpreter.AddCommand(new Status());

            
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