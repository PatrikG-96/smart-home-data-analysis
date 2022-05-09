using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class PositionSample : Sample
    {

        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }

        public PositionSample() : base("Position")
        {
            ParentSource = "WideFind";
        }

        public override string ToString()
        {
            return $"{SampleType};{PosX};{PosY};{PosZ}";
        }

        public override void Reset()
        {
            PosX = PosY = PosZ = null;
        }

        public override Sample Copy()
        {
            return new PositionSample()
            {
                PosX = this.PosX,
                PosY = this.PosY,
                PosZ = this.PosZ,
  
            };
        }

        public override void InsertSensorData(ISensor sensor)
        {
            if (sensor is WideFind wideFind)
            {
                var json = wideFind.GetData();

                if (json == null)
                {
                    return;
                }

                var msg = json.ParseMessage();
                PosX = msg?.PosX ?? "0";
                PosY = msg?.PosY ?? "0";
                PosZ = msg?.PosZ ?? "0";
            }
        }
    }
}