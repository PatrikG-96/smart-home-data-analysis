using System.Collections.Generic;

namespace iMotionsImportTools.Sensor
{
    public class WideFindJson
    {
        internal static readonly List<string> FieldNames = new List<string>()
        {
            "version", "posX", "posY", "posZ", "velX", "velY",
            "velZ", "battery", "rssi", "timealive"
        };

        public string host { get; set; }
        public string message { get; set; }
        public string source { get; set; }
        public string time { get; set; }
        public string type { get; set; }
    }
}