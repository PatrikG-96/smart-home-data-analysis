using System;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorStatus : ICommand
    {
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SensorStatus()
        {
            KeyWord = "status";
            Builder = new OutputBuilder();
            Builder.AddTitle("title");
            Builder.AddAttribute("ID");
            Builder.AddAttribute("Connected");
            Builder.AddAttribute("Started");
            Builder.AddAttribute("Updated");
            Builder.AddAttribute("Time alive");
            Builder.AddAttribute("Last message");
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length == 0) // All sensors
            {
                var handles = controller.GetAllHandles();

                foreach (var handle in handles)
                {
                    Builder.BindValue("title", handle.Name);
                    Builder.BindValue("ID", handle.Id);
                    Builder.BindValue("Connected", handle.IsConnected.ToString());
                    Builder.BindValue("Started", handle.IsStarted.ToString());
                    Builder.BindValue("Updated", handle.TimeSinceLastMessage.ToString());
                    Builder.BindValue("Time alive", handle.TimeAlive.ToString());
                    Builder.BindValue("Last message", handle.LastMessage);
                    Console.WriteLine(Builder.Build());
                }
            }
        }
    }
}