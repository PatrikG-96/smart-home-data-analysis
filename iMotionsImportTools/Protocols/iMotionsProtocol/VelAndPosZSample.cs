using System;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class VelAndPosZSample : Sample
    {

        public string VelX { get; set; }
        public string VelY { get; set; }
        public string VelZ { get; set; }

        public string PosZ { get; set; }

        public VelAndPosZSample() : base("VelAndPosZ")
        {
            ParentSource = "WideFind";
        }

        public override string ToString()
        {
            return $"{SampleType};{VelX};{VelY};{VelZ};{PosZ}";
        }

        public override void Reset()
        {
            VelX = VelY = VelZ = PosZ = null;
        }

        public override Sample Copy()
        {
            return new VelAndPosZSample()
            {

                VelX = this.VelX,
                VelY = this.VelY,
                VelZ = this.VelZ,
                PosZ = this.PosZ
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

                VelX = msg?.VelX ?? "0";
                VelY = msg?.VelY ?? "0";
                VelZ = msg?.VelZ ?? "0";
                PosZ = msg?.PosZ ?? "0";

            }
        }
    }
}