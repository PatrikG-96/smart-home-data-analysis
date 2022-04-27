using System.Collections.Generic;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.Controller
{
    public class SensorWithSampleSubscribers
    {
        
        public ISensor Sensor { get; private set; }
        public List<string> SubscriberIds { get; }

        public SensorWithSampleSubscribers(ISensor sensor)
        {
            Sensor = sensor;
            SubscriberIds = new List<string>();
        }

        public void AddSubscribingSample(string sampleId)
        {
            SubscriberIds.Add(sampleId);
        }

        public void RemoveSubscribingSample(string sampleId)
        {
            SubscriberIds.Remove(sampleId);
        }

    }
}