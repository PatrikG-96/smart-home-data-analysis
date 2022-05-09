using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.iMotionsProtocol
{
    public abstract class Sample
    {

        public string Id { get; set; }
        public string ParentSource { get; set; }

        public string Instance { get; set; } = "";
        public string SampleType { get; protected set;}

        protected Sample(string sampleType)
        {
            SampleType = sampleType;
        }

        public abstract override string ToString();

        public abstract void Reset();

        public abstract Sample Copy();

        public abstract void InsertSensorData(ISensor sensor);

    }
}
