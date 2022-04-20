namespace iMotionsImportTools.Sensor
{
    public abstract class AbstractSensor
    {

        public string Id { get; set; }

        public abstract bool Connect();

        public abstract bool Disconnect();

        public abstract void Start();

        public abstract void Stop();

    }
}
