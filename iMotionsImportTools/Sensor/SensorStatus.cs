using System.Collections.Generic;

namespace iMotionsImportTools.Sensor
{
    public class SensorStatus
    {
        public string Name {get; set;}

        public bool IsConnected { get; set; }

        public bool IsStarted { get; set; }

        public string Id { get; set; }

        public string LastMessage { get; set; }

        public long TimeSinceLastMessage { get; set; }

        public long TimeAlive { get; set; }

        public Dictionary<string, string> Optional { get; } = new Dictionary<string, string>();

    }
}