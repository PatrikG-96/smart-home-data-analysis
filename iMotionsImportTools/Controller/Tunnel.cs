
using iMotionsImportTools.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.Controller
{
    public class Tunnel
    {

        private readonly Func<string, string> _parser;
        private readonly IOutputDevice _client;
        private readonly ITunneler _tunneler;

        private bool _isClosed;

        public Tunnel(ITunneler tunneler, IOutputDevice client, Func<string,string> parser)
        {
            tunneler.Transport += Forward;
            tunneler.ShouldTunnel = true;
            _tunneler = tunneler;
            _client = client;
            _parser = parser;
        }

        public Tunnel(ITunneler tunneler, IOutputDevice client) : this(tunneler, client, null) { }

        private void Forward(object sender, Sample sample)
        {
            if (_isClosed) return;

           
            var message = new Message
            {
                Source = sample.ParentSource, 
                Type = Message.Event, 
                Version = Message.DefaultVersion, 
                Instance = "Tunneled",
                Sample = sample.Copy() // to avoid race conditions
            }; 
            _client.Write(message.ToString());
            
        }

        public void Open()
        {
            _isClosed = false;
            _tunneler.ShouldTunnel = true;
        }

        public void Close()
        {
            _isClosed = true;
            _tunneler.ShouldTunnel = false;
        }
    }
}
