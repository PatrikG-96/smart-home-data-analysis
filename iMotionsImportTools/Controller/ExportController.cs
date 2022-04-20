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


namespace iMotionsImportTools.Controller
{
    public class ExportController
    {

        private readonly Dictionary<string, IExportable> _exportables;
        

        private AsyncTcpClient _client;

        private Dictionary<int, Tunnel> _tunnels;

        public ExportController(AsyncTcpClient client)
        {
            _exportables = new Dictionary<string, IExportable>();
            
            _client = client;
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
