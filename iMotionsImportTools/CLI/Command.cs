using System;

namespace iMotionsImportTools.CLI
{
    public class Command
    {
        
        public string Keyword { get; private set; }

        public Action<string[]> CommandAction { get; private set; }

        public Command(string keyword, Action<string[]> commandAction)
        {
            Keyword = keyword;
            CommandAction = commandAction;
        }

    }
}