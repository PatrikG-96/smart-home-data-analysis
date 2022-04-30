using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI
{
    public class Command
    {
        
        public string Keyword { get; set; }

        public OutputBuilder Builder { get; set; }

        public Action<IMotionsController, string[], OutputBuilder> Function { get; set; }

        public Command(string keyword, OutputBuilder builder, Action<IMotionsController, string[], OutputBuilder> function)
        {
            Keyword = keyword;
            Builder = builder;
            Function = function;
        }
    }
}