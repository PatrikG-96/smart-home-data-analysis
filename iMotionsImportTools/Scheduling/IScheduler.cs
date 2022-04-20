using System;
using iMotionsImportTools.ImportFunctions;

namespace iMotionsImportTools.Scheduling
{
    public interface IScheduler
    {
        event EventHandler<SchedulerEventArgs> Events;
        void Start();

        void Stop();
    }
}
