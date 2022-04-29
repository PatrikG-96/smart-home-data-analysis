using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class FibaroEntranceSample : FibaroSample
    {

        public string DoorMotion { get; set; }
        public string DoorTemp { get; set; }
        public string DoorLight { get; set; }
        public string DoorHumidity { get; set; }

        public FibaroEntranceSample() : base("Entrance")
        {
        }

        public override string ToString()
        {
            return $"{SampleType};{DoorMotion};{DoorTemp};{DoorLight};{DoorHumidity}";
        }

        public override void Reset()
        {
            DoorHumidity = DoorLight = DoorMotion = DoorTemp = null;
        }

        public override Sample Copy()
        {
            return new FibaroEntranceSample
            {
                DoorMotion = DoorMotion,
                DoorHumidity = DoorHumidity,
                DoorLight = DoorLight,
                DoorTemp = DoorTemp
            };
        }

        public override void InsertSensorData(ISensor sensor)
        {
            if (sensor is FibaroSensor fib)
            {
                DoorMotion = fib.GetDeviceInfo(FibaroDevices.U121_DOOR_MOTION).Value;
                DoorTemp = fib.GetDeviceInfo(FibaroDevices.U121_DOOR_TEMP).Value;
                DoorLight = fib.GetDeviceInfo(FibaroDevices.U121_DOOR_LIGHT).Value;
                DoorHumidity = fib.GetDeviceInfo(FibaroDevices.U121_DOOR_HUMIDITY).Value;
            }
        }
    }
}