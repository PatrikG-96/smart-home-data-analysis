using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI
{
    public interface ICommand
    {
        
        string Keyword { get; }

        void Action(IMotionsController controller, string[] args);

    }
}