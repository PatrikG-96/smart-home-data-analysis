using System;
using System.Runtime.InteropServices;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools.CLI
{
    public class Status : ICommand
    {
        public string Keyword { get; }
        private OutputBuilder builder;
        public Status()
        {
            Keyword = "status";
            builder = new OutputBuilder(OutputBuilder.NormalLength);
        }

        public void Action(IMotionsController controller, string[] args)
        {
            
            if (args.Length == 0)
            {
                
                var statuses = controller.GetAllSensorStatuses();

                foreach (var status in statuses)
                {
                   
                    builder.AddLine('-', '#');
                    builder.AddAttribute(status.Name, Formatter.CENTER, '-', '#', ' ');
                    builder.AddLine('-', '#');
                    builder.AddAttribute($"Id: {status.Id}", Formatter.LEFT, edges:'#', surround:' ');
                    builder.AddAttribute($"Connected: {status.IsConnected.ToString()}", Formatter.LEFT, edges: '#', surround: ' ');
                    builder.AddAttribute($"Started: {status.IsStarted.ToString()}", Formatter.LEFT, edges: '#', surround: ' ');
                    builder.AddAttribute($"Last message: {status.TimeSinceLastMessage.ToString()}ms ago", Formatter.LEFT, edges: '#', surround: ' ');
                    builder.AddAttribute($"Time alive: {status.TimeAlive.ToString()}ms", Formatter.LEFT, edges: '#', surround: ' ');

                    foreach (var optionPair in status.Optional)
                    {
                        builder.AddAttribute($"{optionPair.Key}: {optionPair.Value}", Formatter.LEFT, edges: '#', surround: ' ');
                    }
                    builder.AddLine('-', '#');
                    Console.WriteLine(builder.Output());
                }
            }

            if (args.Length == 1)
            {
                var status = controller.GetSensorStatus(args[1]);
                if (status == null) return;
                builder.AddLine('-', '#');
                builder.AddAttribute(status.Name, Formatter.CENTER, '-', '#', ' ');
                builder.AddLine('-', '#');
                builder.AddAttribute($"Id: {status.Id}", Formatter.LEFT, edges: '#', surround: ' ');
                builder.AddAttribute($"Connected: {status.IsConnected.ToString()}", Formatter.LEFT, edges: '#', surround: ' ');
                builder.AddAttribute($"Started: {status.IsStarted.ToString()}", Formatter.LEFT, edges: '#', surround: ' ');
                builder.AddAttribute($"Last message received: {status.TimeSinceLastMessage.ToString()}ms", Formatter.LEFT, edges: '#', surround: ' ');
                builder.AddAttribute($"Time alive: {status.TimeAlive.ToString()}ms", Formatter.LEFT, edges: '#', surround: ' ');

                foreach (var optionPair in status.Optional)
                {
                    builder.AddAttribute($"{optionPair.Key}: {optionPair.Value}", Formatter.LEFT, edges: '#', surround: ' ');
                }
                builder.AddLine('-', '#');
                Console.WriteLine(builder.Output());
            }
        }

    }
}