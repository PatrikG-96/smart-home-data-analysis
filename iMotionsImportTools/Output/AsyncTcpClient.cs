using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using iMotionsImportTools.logs;
using iMotionsImportTools.Output;
using Serilog;

namespace iMotionsImportTools.Network
{
    public class AsyncTcpClient : IAsyncClient, IDisposable, IOutputDevice, ILogEntity
    {

        public EventHandler<byte[]> OnMessageReceived;
        public EventHandler OnDisconnect;

        private TcpClient _client;
        private Stream _stream;

        private BufferBlock<byte[]> _buffer;
        public bool Connected => _client != null && _client.Connected;
        public bool Receiving { get; set; }

        public string Id { get; set; }

        public string LogName { get; set; }

        // max and min buffer sizes?
        private int BufferSize { get; set; } = 8192;
        

        public AsyncTcpClient(string id = "")
        {
            LogName = "AsyncTcpClient:" + id;
            _buffer = new BufferBlock<byte[]>();

        }


        public Task Send(string data) => Send(Encoding.UTF8.GetBytes(data), new CancellationToken());
        public Task Send(string data, CancellationToken token) => Send(Encoding.UTF8.GetBytes(data), token);

        public async Task Send(byte[] data, CancellationToken token)
        {
            try
            {
                //Log.Logger.Debug("Sending message: {A}", Encoding.Default.GetString(data));
                //Console.WriteLine($"Writing data: {Encoding.Default.GetString(data)}");
                await _stream.WriteAsync(data, 0, data.Length, token);
                await _stream.FlushAsync(token);
            }
            catch (IOException e)
            {
                var onDc = OnDisconnect;
                // If writing failed because stream/client was disposed
                // Could be for SSL stream authentication
                if (e.InnerException is ObjectDisposedException)
                {
                    Console.WriteLine("shit");
                }
                // Otherwise we probably are disconnected
                else
                {
                    onDc?.Invoke(this, EventArgs.Empty);
                }
            }

        }

        public async Task Start(CancellationToken token)
        {
            while (Connected)
            {
                var data = await _buffer.ReceiveAsync(token);
                
                await Send(data, token);
            }
        }

        public async Task Connect(ServerInfo info, CancellationToken token)
        {
            try
            {
                await CloseFromAsync();
                token.ThrowIfCancellationRequested();
                _client = new TcpClient();
                token.ThrowIfCancellationRequested();
                await _client.ConnectAsync(info.Address, info.Port);
                await CloseIfCanceled(token);
                _stream = _client.GetStream();
                _stream.WriteTimeout = 100;
            }
            catch (Exception)
            {
                // If connection failed, if it was because of a cancellation request, close
                // everything. Resend exception regardless.
                CloseIfCanceled(token).Wait(token);
                throw;
            }
            
        }

        private async Task CloseFromAsync()
        {
            await Task.Yield();
            Close();
        }

        private void Close()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        private async Task CloseIfCanceled(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                await CloseFromAsync();
                token.ThrowIfCancellationRequested();
            }
        }

        public async Task Receive(CancellationToken token)
        {
            if (!Connected || Receiving)
            {
                throw new Exception();
            }

            Receiving = true;

            byte[] readBuffer = new byte[BufferSize];

            while (Connected)
            {
                token.ThrowIfCancellationRequested();
                int bytesRead = await _stream.ReadAsync(readBuffer, 0, readBuffer.Length, token);
                

                // For my purposes, 8192 bytes should be enough?
                var onMessage = OnMessageReceived;
                var data = new byte[bytesRead];
                Array.Copy(readBuffer, data, bytesRead);
                onMessage?.Invoke(this, data);

                readBuffer = new byte[BufferSize];
            }
        }

        public void Dispose()
        {
            Close();
        }

        

        public void Write(string message)
        {
            _buffer.Post(Encoding.UTF8.GetBytes(message));
        }

        
    }
}