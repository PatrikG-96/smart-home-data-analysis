using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;
using iMotionsImportTools.Protocols;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;
using Serilog;

namespace iMotionsImportTools.Controller
{
    public class SensorController
    {

        private Stopwatch _timestamper;

        private IOutputDevice _output;
        private IScheduler _exportScheduler;
        private IProtocol _protocol;

        private readonly List<SensorSampleSubs> _sensors;
        private readonly Dictionary<string, Sample> _samples;
        private readonly Dictionary<string, IScheduler> _schedulers;
        private readonly Dictionary<int, Tunnel> _tunnels;

        private readonly CancellationTokenSource _controllerTokenSource;

        public bool IsStarted { get; private set; }
        public bool IsConnected { get; private set; }


        public SensorController(IOutputDevice output, IProtocol protocol)
        {
            _output = output;
            _protocol = protocol;
            _controllerTokenSource = new CancellationTokenSource();

            _samples = new Dictionary<string, Sample>();
            _sensors = new List<SensorSampleSubs>();
            _tunnels = new Dictionary<int, Tunnel>();
            _schedulers = new Dictionary<string, IScheduler>();
        }

        /*---------------------------------------*
         *               SENSORS                 *
         *---------------------------------------*/

        public void ConnectAll()
        {
            if (_output == null) return;

            foreach (var sensorWrapper in _sensors)
            {
                if (!sensorWrapper.Handle.Sensor.Connect()) Log.Logger.Warning("Failed to connected sensor '{A}'", sensorWrapper.Handle.Id);
            }

            IsConnected = true;
        }

        public void DisconnectAll()
        {
            if (_output == null) return;
            foreach (var sensorWrapper in _sensors)
            {
                sensorWrapper.Handle.Sensor.Disconnect();
            }
            IsConnected = false;
        }

        public void StartAll()
        {
            if (_output == null) return;
            foreach (var sensorWrapper in _sensors)
            {
                sensorWrapper.Handle.Sensor.Start();
            }

            foreach (var pair in _schedulers)
            {
                pair.Value.Start();
            }
            _exportScheduler?.Start();
            IsStarted = true;
            _timestamper = Stopwatch.StartNew();
        }

        public void StopAll()
        {
            if (_output == null) return;
            foreach (var sensorWrapper in _sensors)
            {
                sensorWrapper.Handle.Sensor.Stop();
            }
            foreach (var pair in _schedulers)
            {
                pair.Value.Stop();
            }
            _exportScheduler?.Stop();
            IsStarted = false;
            _timestamper.Stop();
        }

        public void AddSensor(ISensor sensor)
        {

            _sensors.Add(new SensorSampleSubs(sensor));
        }

        public void RemoveSensor(ISensor sensor)
        {
            foreach (var sensorSub in _sensors)
            {
                if (sensorSub.Handle.Sensor.Id == sensor.Id)
                {
                    _sensors.Remove(sensorSub);
                    return;
                }
            }
        }

        /*---------------------------------------*
         *               SAMPLES                 *
         *---------------------------------------*/


        public void AddSample(string id, Sample sample)
        {
            _samples.Add(id, sample);
        }

        public void RemoveSample(string id)
        {
            _samples.Remove(id);
        }


        public void AddSampleSensorSubscription(string sampleId, string sensorId)
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (sensorWrapper.Handle.Id == sensorId)
                {
                    if (!_samples.ContainsKey(sampleId)) throw new Exception();
                    sensorWrapper.AddSubscribingSample(sampleId);
                    return;
                }
            }

            throw new Exception();
        }

        public void RemoveSampleSensorSubscription(string sampleId, string sensorId)
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (sensorWrapper.Handle.Id == sensorId)
                {
                    sensorWrapper.RemoveSubscribingSample(sampleId);
                    return;
                }
            }
            throw new Exception();
        }


        /*---------------------------------------*
         *             SCHEDULING                *
         *---------------------------------------*/

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

        /*---------------------------------------*
         *               TUNNELS                 *
         *---------------------------------------*/

        public void AddTunnel(int id, ITunneler tunneler)
        {
            var tunnel = new Tunnel(tunneler, _output, _protocol);
            _tunnels.Add(id, tunnel);
            tunnel.Open();
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

        /*--------------------------------------*
         *              GETTERS                 *
         *--------------------------------------*/

        public string GetActiveOutputDeviceId()
        {
            return _output.Id;
        }

        public ISensor GetSensor(string id)
        {
            foreach (var sensorWrapper in _sensors)
            {
                if (sensorWrapper.Handle.Id == id)
                {
                    return sensorWrapper.Handle.Sensor;
                }
            }

            return null;
        }

        public Sample GetSample(string id)
        {
            return _samples[id];
        }

        public List<SensorHandle> GetAllHandles()
        {
            var list = new List<SensorHandle>();
            foreach (var s in _sensors)
            {
                list.Add(s.Handle);
            }

            return list;
        }

        public SensorHandle GetHandle(string sensorId)
        {
            foreach (var sensorWrapper in _sensors)
            {

                if (sensorWrapper.Handle.Id == sensorId)
                {
                    return sensorWrapper.Handle;
                }
            }

            return null;
        }


        /*--------------------------------------*
         *               SETTERS                *
         *--------------------------------------*/

        public void SetOutputDevice(IOutputDevice device)
        {
            _output = device;
        }

        public void SetExportScheduler(IScheduler scheduler)
        {
            _exportScheduler = scheduler;
            _exportScheduler.Events += ExportAll;
        }


        /*--------------------------------------*
         *             CONTROLLER               *
         *--------------------------------------*/


        public void Quit()
        {
            if (_output == null) return;
            StopAll();
            DisconnectAll();
            _controllerTokenSource.Cancel();
        }

        public void OnDisconnect(object sender, EventArgs args)
        {
            Log.Warning("Output device is disconnected. Stopping sensors");
            StopAll();
        }

        

        // Called by the scheduler.
        private void ExportAll(object sender, SchedulerEventArgs args)
        {
            if (_output == null)
            {
                throw new Exception(); // OutputException
            }

            var modifiedSamples = new HashSet<Sample>(); // To keep track of samples that have actually been modified

            foreach (var sensorWrapper in _sensors)
            {
                var handle = sensorWrapper.Handle;
                foreach (var sampleId in sensorWrapper.SubscriberIds) // for each sample that is subscribed to this sensor
                {
                    var sample = _samples[sampleId];
                    try
                    {
                        sample.InsertSensorData(handle.Sensor); // let the sample add the data it wants
                        modifiedSamples.Add(sample);  // no duplicates due to HashSet
                        Log.Logger.Debug("Exported from sensor '{A}'. Added data to sample '{B}'", handle.Id, sampleId);
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
       
                long timestamp = _timestamper.ElapsedMilliseconds;
                _output.Write(_protocol.SampleToMessage(sample, timestamp));
            }

            // reset all samples to, might not be needed
            foreach (var sample in modifiedSamples)
            {
                sample.Reset();
            }

        }

        
    }
}