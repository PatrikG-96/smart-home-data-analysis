using System.Collections.Generic;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.Controller
{
    public class SensorSampleSubs
    {
        
        public SensorHandle Handle { get; private set; }
        public List<string> SubscriberIds { get; }

        public SensorSampleSubs(ISensor sensor)
        {
            Handle = new SensorHandle(sensor);
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