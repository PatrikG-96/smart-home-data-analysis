using System;
using System.Collections.Generic;
using System.Linq;

namespace iMotionsImportTools.ImportFunctions
{
    public class TestScheduledApiSensor : ScheduledApiSensor, IExportable
    {
        public TestScheduledApiSensor(ApiService service, IScheduler scheduler) : base(service, scheduler)
        {
        }

        public Dictionary<string, string> Export()
        {
            Console.WriteLine(Data.ToString());
            return null;
        }
    }
}