using System;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class VelocitySample : Sample
    {

        public string VelX { get; set; }
        public string VelY { get; set; }
        public string VelZ { get; set; }

        public VelocitySample() : base("Velocity")
        {
            ParentSource = "WideFind";
        }

        public override string ToString()
        {
            return $"{SampleType};{VelX};{VelY};{VelZ}";
        }

        public override void Reset()
        {
            VelX = VelY = VelZ = null;
        }

        public override Sample Copy()
        {
            return new VelocitySample()
            {

                VelX = this.VelX,
                VelY = this.VelY,
                VelZ = this.VelZ
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

            }
        }
    }
}