using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotionsImportTools.Sensor
{
    public class Handler
    {

        private List<AbstractSensor> _sensors;

        public Handler()
        {
            _sensors = new List<AbstractSensor>();
        }

        public void ConnectAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Connect();
            }
        }

        public void DisconnectAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Disconnect();
            }
        }

        public void StartAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Start();
            }
        }

        public void StopAll()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Stop();
            }
        }

        public void AddSensor(AbstractSensor sensor)
        {
            _sensors.Add(sensor);
        }

    }
}
