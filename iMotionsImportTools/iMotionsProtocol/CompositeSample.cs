using System.Collections.Generic;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.iMotionsProtocol
{
    public abstract class CompositeSample : Sample
    {


        protected CompositeSample(string sampleType) : base(sampleType)
        {
            ParentSource = "Composite";
        }

        
    }
}