using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotionsImportTools.iMotionsProtocol
{
    public abstract class Sample
    {

        public string ParentSource { get; set; }
        public string SampleType { get; protected set;}

        protected Sample(string sampleType)
        {
            SampleType = sampleType;
        }

        public abstract override string ToString();

    }
}
