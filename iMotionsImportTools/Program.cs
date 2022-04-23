using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Exports;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


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

        public async Task Connect(ServerInfo info, CancellationToken token)
        {
            await Console.Out.WriteAsync("hej");
        }

        public async Task Receive(CancellationToken token)
        {
            await Console.Out.WriteAsync("hej");
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            var wideFind = new WideFind("1", "130.240.74.55");
            wideFind.AddTopic("ltu-system/#");
            wideFind.AddType("REPORT");
            //wideFind.AddType("BEACON");
            wideFind.Tag = "B30D92054F2BF398";

            

            //https://gorest.co.in/public/v2/users
            var client = new AsyncTcpClient();


            var controller = new SensorController(client, new IntervalScheduler(1000), CancellationToken.None);
            controller.AddSensor("1",wideFind, true);
            controller.AddTunnel(1, wideFind);
            Console.WriteLine("hej1");
            client.Connect(new ServerInfo("127.0.0.1", 8089), CancellationToken.None).Wait();
            Console.WriteLine("hej2");
            Task.Run(async () =>
            {
                await client.Receive(CancellationToken.None);
            });
            controller.ConnectAll();
            controller.StartAll();

            Console.ReadKey();

            controller.StopAll();
            //controller.DisconnectAll();
            
            Console.ReadKey();

            controller.StartAll();

            Console.ReadKey();

            controller.Quit();

            Console.ReadKey();
           

      
/**
           var fib = new Fibaro("1", "130.240.74.55", "your_mqtt_username", "your_mqtt_password");
           fib.AddTopic("homeassistant/sensor/68/#");
           fib.AddTopic("homeassistant/sensor/66/#");
           fib.AddTopic("homeassistant/sensor/67/#");
           fib.Connect();
           fib.Start();**/
        }

     
  

    }
}
