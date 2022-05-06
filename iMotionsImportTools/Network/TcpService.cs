using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using iMotionsImportTools.Output;

namespace iMotionsImportTools.Network
{
    public class TcpService : IOutputDevice
    {

        private BufferBlock<string> _buffer;
        private AsyncTcpClient _client;
        private Thread _thread;

        public TcpService(AsyncTcpClient client)
        {
            _client = client;
            _buffer = new BufferBlock<string>();
            _thread = new Thread(Run);
        }

        public void Put(string msg)
        {
            
            _buffer.Post(msg);
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _thread.Abort();
        }

        private async void Run()
        {
            while (true)
            {
                var msg = await _buffer.ReceiveAsync();
                await _client.Send(msg);
            }
        }

        public string Id { get; set; }
        public void Write(string message)
        {
            Console.WriteLine("Sending " + message);
            _buffer.Post(message);
        }
    }
}