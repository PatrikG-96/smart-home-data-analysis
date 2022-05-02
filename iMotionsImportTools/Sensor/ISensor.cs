using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace iMotionsImportTools.Sensor
{

    //TODO:
    // Some interface method for retrieving data. Maybe generics to allow for different data types for different sensors? (ISensor<T>)
    public interface ISensor
    {

        string Id { get; set; }
        bool IsStarted { get;}

        Stopwatch MessageReceivedWatch { get; }
        Stopwatch TimeAliveWatch { get; }
        string Data { get; }

        bool IsConnected { get; }

        bool Connect(string username=null, string password=null);

        bool Disconnect();

        void Start();

        void Stop();

        SensorStatus Status();

    }
}
