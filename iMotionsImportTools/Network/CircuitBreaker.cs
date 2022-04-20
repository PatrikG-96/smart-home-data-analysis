using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace iMotionsImportTools.ImportFunctions
{
    public class CircuitBreaker
    {
        public SemaphoreSlim Sem { get; }
        private readonly Timer _timer;
        
        private const long Open = 0;
        private const long Closed = 1;
        private const long HalfOpen = 2;
        private long _circuitState;

        
        public CircuitBreaker(int maxQueueSize, int resetTime)
        {
            
            Sem = new SemaphoreSlim(maxQueueSize);
            _timer = new Timer(interval: resetTime);
            _timer.AutoReset = false;
            _timer.Elapsed += OnElapsedEvent;

            _circuitState = Closed;
        }
        

        public void OpenCircuit(string reason = null)
        {
            if (Interlocked.CompareExchange(ref _circuitState, Open, Closed) == Closed)
            {
                if (reason!= null) Console.WriteLine("Opening circuit for reason: " + reason);
            }
            _timer.Start();
        }

        public void CloseCircuit()
        {
            if (Interlocked.CompareExchange(ref _circuitState, Closed, Open) == Open)
            {
                Console.WriteLine("Closing circuit...");
            }
        }

        public bool IsOpen()
        {
            return Interlocked.Read(ref _circuitState) == Open;
        }

        private void OnElapsedEvent(object sender, ElapsedEventArgs args)
        {
            CloseCircuit();
        }
    }
}
