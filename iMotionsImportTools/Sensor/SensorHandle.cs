using System.Collections.Generic;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.Sensor
{
    public class SensorHandle
    {

        public ISensor Sensor { get; private set; }
        public SensorHandle(ISensor sensor)
        {
            Sensor = sensor;
        }

        public string Name => Sensor.GetType().Name;

        public bool IsConnected => Sensor.IsConnected;

        public bool IsStarted => Sensor.IsStarted;

        public string Id => Sensor.Id;

        public string LastMessage => Sensor.Data;

        public long TimeSinceLastMessage => Sensor.MessageReceivedWatch?.ElapsedMilliseconds ?? 0;

        public long TimeAlive => Sensor.TimeAliveWatch?.ElapsedMilliseconds ?? 0;

        public Dictionary<string, string> Optional
        {
            get
            {
                var dict = new Dictionary<string, string>();
                if (Sensor is WideFind.WideFind wideFind)
                {
                    dict.Add("Tag", wideFind.Tag);
                }

                return dict;
            }
        } 
    }
}