
using iMotionsImportTools.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Output;
using iMotionsImportTools.Protocols;

namespace iMotionsImportTools.Controller
{
    public class Tunnel
    {

        private readonly Func<string, string> _parser;
        private readonly IOutputDevice _client;
        private readonly ITunneler _tunneler;
        private readonly IProtocol _protocol;
        private  Stopwatch _timestamper;

        private bool _isClosed;

        public Tunnel(ITunneler tunneler, IOutputDevice client, IProtocol protocol)
        {
            tunneler.Transport += Forward;
            tunneler.ShouldTunnel = true;
            _tunneler = tunneler;
            _client = client;
            _protocol = protocol;
        }

        public Tunnel(ITunneler tunneler, IOutputDevice client) : this(tunneler, client, null) { }

        private void Forward(object sender, Sample sample)
        {
            if (_isClosed) return;

            long timestamp = _timestamper.ElapsedMilliseconds;
            _client.Write(_protocol.SampleToMessage(sample, timestamp));
            
        }


        public void Open()
        {
            _isClosed = false;
            _tunneler.ShouldTunnel = true;
            _timestamper = Stopwatch.StartNew();
        }

        public void Close()
        {
            _isClosed = true;
            _tunneler.ShouldTunnel = false;
        }
    }
}
