using System.Collections.Generic;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI
{
    public class Interpreter
    {

        private readonly List<ICommand> _commands;

        public Interpreter()
        {
            _commands = new List<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        public void Interpret(string keyword, string[] arguments, IMotionsController controller)
        {
            foreach (var command in _commands)
            {
                if (command.Keyword == keyword)
                {
                    command.Action(controller, arguments);
                }
            }
        }
    }
}