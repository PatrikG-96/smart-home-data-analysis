using System;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Utilities;

namespace iMotionsImportTools.Scheduling
{
    public class IntervalScheduler : IScheduler
    {
        public event EventHandler<SchedulerEventArgs> Events;

        private readonly HighResolutionTimer _timer;

        public IntervalScheduler(int interval)
        {
            _timer = new HighResolutionTimer(interval);
            _timer.Elapsed += OnElapsedEvent;
        }
        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnElapsedEvent(object sender, HighResolutionTimerElapsedEventArgs args)
        {
            EventHandler<SchedulerEventArgs> handler = Events;
            handler?.Invoke(this, null);
        }
    }
}
