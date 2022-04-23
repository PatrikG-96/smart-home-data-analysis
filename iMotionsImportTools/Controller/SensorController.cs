using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using iMotionsImportTools.Exports;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;


namespace iMotionsImportTools.Controller
{
    public class SensorController
    {

        private readonly Dictionary<string, ISensor> _sensors;

        private readonly List<IExportable> _exportables;

        private CancellationToken _globalToken;

        private IClient _client;

        private Dictionary<int, Tunnel> _tunnels;

        private IScheduler _scheduler;

        public SensorController(IClient client, IScheduler scheduler, CancellationToken globalToken)
        {
            _sensors = new Dictionary<string, ISensor>();

            _exportables = new List<IExportable>();

            _tunnels = new Dictionary<int, Tunnel>();
            
            _client = client;

            _globalToken = globalToken;

            _scheduler = scheduler;
        }

        public CancellationToken GlobalToken => _globalToken;
        public void ConnectAll()
        {
            Console.WriteLine("conencting");
            foreach (var sensor in _sensors)
            {
                if (!sensor.Value.Connect()) Console.WriteLine("connecting failed");
            }
        }

        public void DisconnectAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Value.Disconnect();
            }
        }

        public void StartAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Value.Start();
            }
            _scheduler.Start();
        }

        public void StopAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Value.Stop();
            }
            _scheduler.Stop();
        }
        public void AddTunnel(int id, ITunneler tunneler)
        {
            _tunnels.Add(id, new Tunnel(tunneler, _client));
        }

        public void RemoveTunnel(int id)
        {
            _tunnels.Remove(id);
        }

        public void OpenTunnel(int id)
        {
            _tunnels[id].Open();
        }

        public void CloseTunnel(int id)
        {
            _tunnels[id].Close();
        }

        public void OpenAllTunnels()
        {
            foreach (var pair in _tunnels)
            {
                pair.Value.Open();
            }
        }

        public void CloseAllTunnels()
        {
            foreach (var pair in _tunnels)
            {
                pair.Value.Close();
            }
        }

        public void AddSensor(string id, ISensor sensor, bool shouldSchedule = false)
        {
            _sensors.Add(id, sensor);

            if (sensor is IExportable export)
            {
                _exportables.Add(export);
            }

            if (!shouldSchedule) return;

            if (sensor is ISchedulable schedulable)
            {
                _scheduler.Events += schedulable.OnScheduledEvent;
            }
            else
            {
                throw new Exception("not schedulable");
            }
        }

        public void RemoveSensor(string id)
        {
            if (!_sensors.TryGetValue(id, out var sensor)) return;


            if (sensor is IExportable export)
            {
                _exportables.Remove(export);
            }

            if (sensor is ISchedulable schedulable)
            {
                _scheduler.Events -= schedulable.OnScheduledEvent;
            }

            _sensors.Remove(id);

            
        }

        public void Quit()
        {
            StopAll();
            DisconnectAll();
            // scuffed fix later
            var src = CancellationTokenSource.CreateLinkedTokenSource(_globalToken);
            src.Cancel();
        }

        public void ExportAll(object sender, SchedulerEventArgs args)
        {
            foreach (var export in _exportables)
            {
                
                var value = export.Export().StringRepr();
                //Console.WriteLine("Exported data ('"+ export.Key + "'): " + value);
                var task = Task.Run(async () =>
                {
                    await _client.Send(value, new CancellationToken());
                    //Console.WriteLine("Sent: " + value);
                });
                
                
            }
        }
        
        
    }
}
