using iMotionsImportTools.Sensor;

namespace tests.Mocks
{
    public class MockSensor : ISensor
    {
        private int[] _output;
        private int _outputPosition = 0;


        public string Id { get; set; }
        public bool IsStarted { get; private set; }
        public bool IsConnected { get; private set; }

        public MockSensor(int[] outputArray)
        {
            _output = outputArray;
        }

        public bool Connect()
        {
            IsConnected = true;
            return IsConnected;
        }

        public bool Disconnect()
        {
            IsConnected = false;
            return IsConnected;
        }

        public void Start()
        {
            IsStarted = true;
        }

        public void Stop()
        {
            IsStarted = false;
        }

        public int GetData()
        {
            var data = _output[_outputPosition];
            _outputPosition++;
            return data;
        }
    }
}