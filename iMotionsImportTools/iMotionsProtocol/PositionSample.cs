using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class PositionSample : WideFindSample
    {

        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }

        public PositionSample() : base("Position")
        {
        }

        public override string ToString()
        {
            return $"{SampleType};{PosZ};{PosY};{PosX}";
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
                PosX = msg.PosX;
                PosY = msg.PosY;
                PosZ = msg.PosZ;
            }
        }
    }
}