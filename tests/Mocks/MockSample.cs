using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Sensor;

namespace tests.Mocks
{
    public class MockSample : Sample
    {

        public int Attribute { set; get; }

        public MockSample() : base("MockSample")
        {
        }

        public override string ToString()
        {
            return $"{SampleType};{Attribute}";
        }

        public override void Reset()
        {
            Attribute = 0;
        }

        public override Sample Copy()
        {
            return new MockSample
            {
                Attribute = Attribute
            };
        }

        public override void InsertSensorData(ISensor sensor)
        {
            if (sensor is MockSensor mock)
            {
                Attribute = mock.GetData();
            }
        }
    }
}