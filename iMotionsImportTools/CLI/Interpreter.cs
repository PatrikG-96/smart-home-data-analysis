using System;
using System.Collections.Generic;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI
{
    public class Interpreter
    {

        private readonly List<Command> _commands;

        public Interpreter()
        {
            _commands = new List<Command>();
        }

        public void AddCommand(Command command)
        {
            _commands.Add(command);
        }

        public void Interpret(string keyword, string[] arguments, IMotionsController controller)
        {
            foreach (var command in _commands)
            {
                if (command.Keyword == keyword)
                {
                   
                    command.Function(controller, arguments, command.Builder);
                }
            }
        }
    }
}