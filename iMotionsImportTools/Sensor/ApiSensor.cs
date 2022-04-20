using System.Collections.Generic;
using iMotionsImportTools.Network;

namespace iMotionsImportTools.Sensor
{
    public abstract class ApiSensor : AbstractSensor
    {
        protected readonly ApiService ApiService;
        protected List<string> Routes;

        protected ApiSensor(ApiService service)
        {
            ApiService = service;
            Routes = new List<string>();
        }

        public void AddRoute(string route)
        {
            Routes.Add(route);
        }

        public void RemoveRoute(string route)
        {
            Routes.Remove(route);
        }

        public override bool Connect()
        {
            // Doesnt make much sense?
            return true;
        }

        public override bool Disconnect()
        {
            // Some better stuff could be made?
            return true;
        }

    }
}