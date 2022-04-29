using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using iMotionsImportTools.ImportFunctions;
using Timer = System.Timers.Timer;

namespace iMotionsImportTools.Controller
{
    public class Controller
    {

        private bool _started;
        private readonly Timer _timer;
        private readonly Dictionary<string, IExportable> _exportables;

        public Controller(int frequency)
        {
            _timer = new Timer(1 / (double)frequency);
            _timer.AutoReset = true;
            _exportables = new Dictionary<string, IExportable>();
            _started = false;
        }

        public void AddExportable(string id, IExportable exportable)
        {
            _exportables.Add(id, exportable);
        }

        public void RemoveExportable(string id)
        {
            _exportables.Remove(id);
        }

        public void Start()
        {
            if (_started)
            {
                return;
            }
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
            _started = true;

        }

        public void Start(int frequency)
        {
            if (_started)
            {
                return;
            }
            _timer.Interval = 1 / (double) frequency;
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
            _started = true;
        }

        public void Stop()
        {
            _timer.Stop();
            _started = false;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            foreach (var pair in _exportables)
            {
                // Enqueue in client code
                pair.Value.Export();
            }
        }
    }
}
