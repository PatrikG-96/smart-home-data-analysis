namespace iMotionsImportTools.Sensor
{
    public class FibaroJson
    {

        public const string UNAVAILABLE = "-1";

        public string Value { get; set; } = UNAVAILABLE;
        public string RoomName { get; set; } = UNAVAILABLE;
        public string DeviceName { get; set; } = UNAVAILABLE;
        public string Timestamp { get; set; } = UNAVAILABLE;
        public int Id { get; set; } = -1;
        public long Created { get; set; } = -1;

        public new string ToString()
        {
            return
                $"ID:{Id}, DeviceName:{DeviceName}, RoomName:{RoomName}, Timestamp:{Timestamp}, Created:{Created}, Value:{Value}";
        }

    }
}