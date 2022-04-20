using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;

namespace iMotionsImportTools.Sensor
{
    public class FibaroDevice : IEquatable<FibaroDevice>
    {
        public string Route { get; set; }
        public string Name { get; set; }

        public FibaroDevice(string name, string route)
        {
            Name = name;
            Route = route;
        }

        public bool Equals(FibaroDevice other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Route == other.Route && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FibaroDevice) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Route != null ? Route.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
    public class Fibaro : ScheduledApiSensor
    {

        private List<FibaroDevice> _devices;

        public Fibaro(ApiService service, IScheduler scheduler) : base(service, scheduler)
        {
            _devices = new List<FibaroDevice>();
        }

        public void AddDevice(FibaroDevice device)
        {
            if (_devices.Contains(device) || Routes.Contains(device.Route))
            {
                return;
            }
            _devices.Add(device);
            Routes.Add(device.Route);
            
        }

        public void RemoveDevice(FibaroDevice device)
        {
            foreach (var dev in _devices)
            {
                if (dev.Equals(device))
                {
                    _devices.Remove(dev);
                    break;
                }
            }
        }


    }
}
