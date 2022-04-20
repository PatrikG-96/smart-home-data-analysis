using System;
using System.Diagnostics;
using System.Timers;
using iMotionsImportTools.Utilities;

namespace iMotionsImportTools.Scheduling
{
    public class PreemptiveScheduler : IScheduler
    {
        public bool Debug { set; get; }
        public int CycleBreakpoint { set; get; }
        public int Cycles { set; get; }

        public event EventHandler<SchedulerEventArgs> Events;
        public event EventHandler<SchedulerEventArgs> ControllerEvents;

        private HighResolutionTimer _baseTimer;
        private HighResolutionTimer _execTimer;

        private Stopwatch driftTracker;
        
        public Stopwatch fullTest { get; set; }

        public int Interval { get; set; }
        public int ExecutionTime { get; set; }
        public long Drift { set; get; }

        public bool IsRunning { set; get; }
        public PreemptiveScheduler(int interval, int executionTime, int driftApproximation = 0)
        {
            Interval = interval;
            ExecutionTime = executionTime;
            _baseTimer = new HighResolutionTimer(interval - executionTime - driftApproximation);
            _execTimer = new HighResolutionTimer(executionTime - driftApproximation);
            _baseTimer.Elapsed += OnElapsedEvent;
            _execTimer.Elapsed += OnControllerElapsedEvent;
        }

        public void Start()
        {
            driftTracker = Stopwatch.StartNew();
            _baseTimer.Start();
            fullTest = Stopwatch.StartNew();
            IsRunning = true;
        }

        public void Stop()
        {
            _baseTimer.Stop();
            _execTimer.Stop();
            fullTest.Stop();
            IsRunning = false;
        }

        private void OnElapsedEvent(object sender, HighResolutionTimerElapsedEventArgs args)
        {
            driftTracker.Stop();
    //        Console.WriteLine("Base timer stopped, elapsed timed: " + driftTracker.ElapsedMilliseconds);
            Drift += driftTracker.ElapsedMilliseconds - (Interval - ExecutionTime);
            driftTracker.Reset();
            driftTracker.Start();
            
            _baseTimer.Stop();
            float nextInterval = ExecutionTime - Drift;
            if (nextInterval < 1)
            {
                nextInterval = 1;
            }
            _execTimer.Interval = nextInterval;
            _execTimer.Start();
            EventHandler<SchedulerEventArgs> ev = Events;
            ev?.Invoke(ev, null);
            
        }

        private void OnControllerElapsedEvent(object sender, HighResolutionTimerElapsedEventArgs args) 
        {

            
            driftTracker.Stop();
//            Console.WriteLine("Exec timer stopped, elapsed timed: " + driftTracker.ElapsedMilliseconds);
            _execTimer.Stop();
            Drift += driftTracker.ElapsedMilliseconds - ExecutionTime;
            float nextInterval = (Interval - ExecutionTime) - Drift;
            if (nextInterval < 1)
            {
                nextInterval = 1;
            }
            _baseTimer.Interval = nextInterval;
            _baseTimer.Start();


            driftTracker.Reset();
            driftTracker.Start();

  //          Console.WriteLine("Cycle complete, elapsed timed: " + testTimer2.ElapsedMilliseconds);
            Cycles += 1;

            if (Debug && CycleBreakpoint > 0 && Cycles == CycleBreakpoint)
            {
                Stop();
                Console.WriteLine("stopped");
            }
            EventHandler<SchedulerEventArgs> ev = ControllerEvents;
            ev?.Invoke(ev, null);
        }
    }
}