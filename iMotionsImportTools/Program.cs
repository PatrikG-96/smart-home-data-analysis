using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.Controller;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;



namespace iMotionsImportTools
{
    class MockClient : IClient
    {
        public Task Send(string data)
        {
            Console.WriteLine(data);
            return new Task(() => Console.WriteLine(data));
        }

        public Task Send(string data, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Send(byte[] data, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Connect(ServerInfo info, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Receive(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            var wideFind = new WideFind("1", "130.240.74.55");
            wideFind.AddTopic("ltu-system/#");
            wideFind.AddType("BEACON");
            wideFind.AddType("REPORT");
            wideFind.Tag = "03FF5C0A2BFA3A9B";
            var handler = new Handler();
            handler.AddSensor(wideFind);
            var client = new AsyncTcpClient();
            client.Connect(new ServerInfo("127.0.0.1", 8089), CancellationToken.None).Wait();
            Task.Run(async () =>
            {
                await client.Receive(CancellationToken.None);
            });
            var tunnel = new Tunnel(wideFind, client);
            handler.ConnectAll();
            handler.StartAll();

            Console.ReadKey();

            handler.DisconnectAll();

            Console.ReadKey();

            

        }

     
  

    }
}
