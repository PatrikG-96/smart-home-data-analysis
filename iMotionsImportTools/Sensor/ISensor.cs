namespace iMotionsImportTools.Sensor
{
    public interface ISensor
    {

        string Id { get; set; }
        bool IsStarted { get;}


        bool IsConnected { get; }

        bool Connect();

        bool Disconnect();

        void Start();

        void Stop();

    }
}
