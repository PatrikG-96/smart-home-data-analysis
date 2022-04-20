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


namespace iMotionsImportTools.Controllers
{
    public class Controller
    {

        private readonly Dictionary<string, IExportable> _exportables;
        private List<IDataCollection> _dataCollections;

        private AsyncTcpClient _client;

        public Controller(AsyncTcpClient client)
        {
            _exportables = new Dictionary<string, IExportable>();
            _dataCollections = new List<IDataCollection>();
            _client = client;
        }

        public void AddExportable(string id, IExportable exportable)
        {
            _exportables.Add(id, exportable);
        }

        public void RemoveExportable(string id)
        {
            _exportables.Remove(id);
        }

        public void ExportAll(object sender, SchedulerEventArgs args)
        {
            foreach (var export in _exportables)
            {
                var value = export.Value.Export().StringRepr();
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
