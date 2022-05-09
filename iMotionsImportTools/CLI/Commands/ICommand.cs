using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands
{
    public interface ICommand
    {
        
        string KeyWord { get; set; }
        OutputBuilder Builder { get;}

        void ExecuteCommand(SensorController controller, string[] args);
    }
}