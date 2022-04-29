using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class PosxAndDoorTemp : CompositeSample
    {
        public string PosX { get; set; }
        public string DoorTemp { get; set; }

        public PosxAndDoorTemp() : base("PosAndTemp")
        {
        }

        public override string ToString()
        {
            return $"{SampleType};{PosX};{DoorTemp}";
        }

        public override void Reset()
        {
            PosX = DoorTemp = null;
        }

        public override Sample Copy()
        {
            return new PosxAndDoorTemp
            {
                PosX = PosX,
                DoorTemp = DoorTemp
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

            }

            if (sensor is FibaroSensor fib)
            {
                DoorTemp = fib.GetDeviceInfo(FibaroDevices.U121_DOOR_TEMP).Value;
            }
        }
    }
}