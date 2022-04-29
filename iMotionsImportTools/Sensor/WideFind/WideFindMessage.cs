namespace iMotionsImportTools.Sensor
{
    public class WideFindMessage
    {
        public const int VelXIndex = 5;
        public const int VelYIndex = 6;
        public const int VelZIndex = 7;
        public const int PosXIndex = 2;
        public const int PosYIndex = 3;
        public const int PosZIndex = 4;
        public const int IdIndex = 0;
        public const int VersionIndex = 1;
        public const int BatteryIndex = 8;
        public const int RssiIndex = 9;
        public const int TimealiveIndex = 10;

        public string Id { get; set; }
        public string VelX { get; set; }
        public string VelY { get; set; }
        public string VelZ { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string Version { get; set; }
        public string Rssi { get; set; }
        public string Battery { get; set; }
        public string Timealive { get; set; }

        public override string ToString()
        {
            return $"{Id},{Version},{PosX},{PosY},{PosZ},{VelX},{VelY}, {VelZ},{Battery},{Rssi},{Timealive}";
        }
    }
}