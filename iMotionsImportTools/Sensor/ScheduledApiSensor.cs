using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using iMotionsImportTools.Exports;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;

namespace iMotionsImportTools.Sensor
{
    public class ScheduledApiSensor : ApiSensor
    {

        protected readonly IScheduler Scheduler;
        protected ConcurrentDictionary<string, string> Data;

        private Stopwatch _stopwatch;

        public ScheduledApiSensor(ApiService service, IScheduler scheduler) : base(service)
        {
            
            Scheduler = scheduler;
            scheduler.Events += OnScheduledEvent;
            Data = new ConcurrentDictionary<string, string>();
        }
        

        public override void Start()
        {
            _stopwatch = Stopwatch.StartNew();
            Scheduler.Start();
        }

        public override void Stop()
        {
            Scheduler.Stop();
        }
        

        private void OnScheduledEvent(object sender, SchedulerEventArgs args)
        {
            _stopwatch.Stop();
            //Console.WriteLine("Elapsed time: " + _stopwatch.ElapsedMilliseconds);
            _stopwatch.Reset();
            _stopwatch.Start();
            foreach (var route in Routes)
            {
                var task = Task.Run(async () =>
                {
                    string result = await ApiService.MakeRequest(route);
                    Data[route] = result;
                });
            }
            
        }
        
    }
}
