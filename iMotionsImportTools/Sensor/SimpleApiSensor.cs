using System.Collections.Generic;
using iMotionsImportTools.Exports;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;

namespace iMotionsImportTools.Sensor
{
    public class SimpleApiSensor : ApiSensor, IExportable
    {



        public SimpleApiSensor(ApiService service) : base(service)
        {
            
        }
        
        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public ExportData Export()
        {
            throw new System.NotImplementedException();
        }
    }
}