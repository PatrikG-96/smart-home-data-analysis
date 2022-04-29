using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;
using Serilog;

namespace iMotionsImportTools.Controller
{
    public class IMotionsController
    {
        private Dictionary<string, IScheduler> _schedulers;

        private readonly List<SensorWithSampleSubscribers> _sensors;

        private readonly Dictionary<string, Sample> _samples;

        private CancellationToken _globalToken;

        private IOutputDevice _client;

        private Dictionary<int, Tunnel> _tunnels;

        private IScheduler _scheduler;


        public IMotionsController(IOutputDevice client, CancellationToken token)
        {
            _client = client;
            _globalToken = token;

            _samples = new Dictionary<string, Sample>();
            _sensors = new List<SensorWithSampleSubscribers>();
            _tunnels = new Dictionary<int, Tunnel>();
            _schedulers = new Dictionary<string, IScheduler>();
        }

        public void ScheduleExports(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _scheduler.Events += ExportAll;
        }


        public void ConnectAll()
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (!sensorWrapper.Sensor.Connect()) Log.Logger.Warning("Failed to connected sensor '{A}'", sensorWrapper.Sensor.Id);
            }
        }

        public void DisconnectAll()
        {
            foreach (var sensorWrapper in _sensors)
            {
                sensorWrapper.Sensor.Disconnect();
            }
        }

        public void StartAll()
        {
            foreach (var sensorWrapper in _sensors)
            {
                sensorWrapper.Sensor.Start();
            }

            foreach (var pair in _schedulers)
            {
                pair.Value.Start();
            }
            _scheduler?.Start();
        }

        public void StopAll()
        {
            foreach (var sensorWrapper in _sensors)
            {
                sensorWrapper.Sensor.Stop();
            }
            foreach (var pair in _schedulers)
            {
                pair.Value.Stop();
            }
            _scheduler?.Stop();
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

        public void AddSensor(ISensor sensor)
        {
            _sensors.Add(new SensorWithSampleSubscribers(sensor));
        }

        public ISensor GetSensor(string id)
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (sensorWrapper.Sensor.Id == id)
                {
                    return sensorWrapper.Sensor;
                }
            }

            return null;
        }

        public void AddScheduler(string name, IScheduler scheduler)
        {
            _schedulers.Add(name, scheduler);
        }

        public void ScheduleSensor(string sensorId, string schedulerName)
        {
            var sensor = GetSensor(sensorId);
            var scheduler = _schedulers[schedulerName];

            if (!(sensor is ISchedulable schedulable) || scheduler == null)
            {
                throw new Exception("Either sensor or scheduler wasn't found");
            }
                
            scheduler.Events += schedulable.OnScheduledEvent;

        }

        public void Quit()
        {
            StopAll();
            DisconnectAll();
            // scuffed fix later
            var src = CancellationTokenSource.CreateLinkedTokenSource(_globalToken);
            src.Cancel();
        }


        public void AddSample(string id, Sample sample)
        {
            _samples.Add(id, sample);
        }

        public void RemoveSample(string id)
        {
            _samples.Remove(id);
        }

        public Sample GetSample(string id)
        {
            return _samples[id];
        }

        public void AddSampleSensorSubscription(string sampleId, string sensorId)
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (sensorWrapper.Sensor.Id == sensorId)
                {
                    sensorWrapper.AddSubscribingSample(sampleId);
                    return;
                }
            }
        }

        public void RemoveSampleSensorSubscription(string sampleId, string sensorId)
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (sensorWrapper.Sensor.Id == sensorId)
                {
                    sensorWrapper.RemoveSubscribingSample(sampleId);
                    return;
                }
            }
        }

        // Called by the scheduler.
        private void ExportAll(object sender, SchedulerEventArgs args)
        {

            Log.Logger.Debug("Exporting...");

            var modifiedSamples = new HashSet<Sample>(); // To keep track of samples that have actually been modified

            foreach (var sensorWrapper in _sensors)
            {
                var sensor = sensorWrapper.Sensor;
                foreach (var sampleId in sensorWrapper.SubscriberIds) // for each sample that is subscribed to this sensor
                {
                    var sample = _samples[sampleId];
                    try
                    {
                        sample.InsertSensorData(sensor); // let the sample add the data it wants
                        modifiedSamples.Add(sample);  // no duplicates due to HashSet
                        Log.Logger.Debug("Exported from sensor '{A}'. Added data to sample '{B}'", sensor.Id, sampleId);
                    }
                    catch (Exception e)
                    {
                        // Should probably never get here as errors are handled elsewhere, but you never know
                        Log.Logger.Warning("Failed to insert sensor data into sample. '{A}'", e.ToString());
                    }
                }

            }

            foreach (var sample in modifiedSamples)
            {
                // message according to iMotions protocol
                var msg = new Message
                {
                    Source = sample.ParentSource,
                    Version = Message.DefaultVersion,
                    Type = Message.Event,
                    Sample = sample.Copy() // to avoid race conditions when resetting samples
                };

                // write the message string to an output device
                _client.Write(msg.ToString());
            }

            // reset all samples to avoid stale data (this is why we copy above)
            foreach (var sample in modifiedSamples)
            {
                sample.Reset();
            }

        }

        public List<SensorStatus> GetAllSensorStatuses()
        {
            var list = new List<SensorStatus>();
            foreach (var sensorWrapper in _sensors)
            {
                list.Add(sensorWrapper.Sensor.Status());
            }

            return list;
        }

        public SensorStatus GetSensorStatus(string sensorId)
        {
            foreach (var sensorWrapper in _sensors)
            {
                Console.WriteLine("hej");
                if (sensorWrapper.Sensor.Id == sensorId)
                {
                    return sensorWrapper.Sensor.Status();
                }
            }

            return null;
        }
    }
}