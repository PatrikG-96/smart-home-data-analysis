using System.Collections.Generic;

namespace iMotionsImportTools.Sensor
{
    // TODO:
    // Move message field indices here
    public class WideFindJson
    {
        public string Host { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }

        public WideFindMessage ParseMessage()
        {
            

            var colonSeparated = Message.Substring(Message.IndexOf(':') + 1);

            var separatedFields = colonSeparated.Split(',');
            return new WideFindMessage
            {
                Id = separatedFields[WideFindMessage.IdIndex],
                Version = separatedFields[WideFindMessage.VersionIndex],
                PosX = separatedFields[WideFindMessage.PosXIndex],
                PosY = separatedFields[WideFindMessage.PosYIndex],
                PosZ = separatedFields[WideFindMessage.PosZIndex],
                VelX = separatedFields[WideFindMessage.VelXIndex],
                VelY = separatedFields[WideFindMessage.VelYIndex],
                VelZ = separatedFields[WideFindMessage.VelZIndex],
                Battery = separatedFields[WideFindMessage.BatteryIndex],
                Rssi = separatedFields[WideFindMessage.RssiIndex],
                Timealive = separatedFields[WideFindMessage.TimealiveIndex]
            };
           
        }
    }
}