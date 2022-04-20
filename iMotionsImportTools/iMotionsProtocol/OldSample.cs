using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class OldSample
    {

        private List<string> _dataFields;

        public OldSample(string sampleType, List<string> dataFields)
        {
            SampleType = sampleType;
            _dataFields = dataFields;
           
        }

        public OldSample(string sampleType) : this(sampleType, new List<string> {}) { }

        public string SampleType { get; private set; }

        public void SetData(List<string> data)
        {
            _dataFields = data;
        }

        public void AddDatafield(string value)
        {
            _dataFields.Add(value);
        }

        public void RemoveDatafield(string name)
        {
            _dataFields.Remove(name);
        }

        public override string ToString()
        {
            return $"{SampleType};{string.Join(";", _dataFields)}";
        }
    }
}
