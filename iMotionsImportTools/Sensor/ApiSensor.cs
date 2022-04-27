using System.Collections.Generic;
using iMotionsImportTools.Network;

namespace iMotionsImportTools.Sensor
{

    // This class is not gonna be needed. Make some minor improvements just to have the support if there is future use cases here
    // TODO:
    // Better connect logic
    public abstract class ApiSensor : ISensor
    {
        protected readonly ApiService ApiService;
        protected List<string> Routes;

        protected ApiSensor(string id, ApiService service)
        {
            ApiService = service;
            Routes = new List<string>();
            Id = id;
        }

        public void AddRoute(string route)
        {
            Routes.Add(route);
        }

        public void RemoveRoute(string route)
        {
            Routes.Remove(route);
        }

        public string Id { get; set; }
        public bool IsStarted { get; private set; }
        public bool IsConnected { get; private set; }

        public bool Connect()
        {
            // Doesnt make much sense?
            IsConnected = true;
            return true;
        }

        public bool Disconnect()
        {
            // Some better stuff could be made?
            IsConnected = false;
            return true;
        }

        public void Start()
        {
            IsStarted = true;
        }

        public void Stop()
        {
            IsStarted = false;
        }
    }
}