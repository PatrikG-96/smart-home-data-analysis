using System;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class VelocitySample : WideFindSample
    {

        public string VelX { get; set; }
        public string VelY { get; set; }
        public string VelZ { get; set; }

        public VelocitySample() : base("Velocity")
        {
        }

        public override string ToString()
        {
            return $"{SampleType};{VelZ};{VelY};{VelX}";
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
                Console.WriteLine("Inserting");
                var json = wideFind.GetData();

                if (json == null)
                {
                    
                    return;
                }
                Console.WriteLine("Inserting");
                var msg = json.ParseMessage();
                
                VelX = msg.VelX;
                VelY = msg.VelY;
                VelZ = msg.VelZ;

            }
        }
    }
}