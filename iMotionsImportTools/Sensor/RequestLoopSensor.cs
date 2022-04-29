using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;

namespace iMotionsImportTools.Sensor
{
    public class RequestLoopSensor : ApiSensor, ISchedulable
    {

        public bool IsScheduled { get; set; }
        protected ConcurrentDictionary<string, string> Data; // route -> data for that route

        public RequestLoopSensor(string id, ApiService service) : base(id, service)
        {
            Data = new ConcurrentDictionary<string, string>();
        }


        public void OnScheduledEvent(object sender, SchedulerEventArgs args)
        {

            if (!IsConnected || !IsStarted)
            {
                return;
            }

            foreach (var route in Routes)
            {
                var task = Task.Run(async () =>
                {
                    string result = await ApiService.MakeRequest(route);
                    Console.WriteLine("Api result: " + result);
                    Data[route] = result;
                });
            }

        }

        public override string Status()
        {
            throw new NotImplementedException();
        }
    }
}