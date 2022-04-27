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

using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;
using Serilog;
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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("my_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var wideFind = new WideFind("1", "130.240.74.55");
            wideFind.AddTopic("ltu-system/#");
            wideFind.AddType("REPORT");
            //wideFind.AddType("BEACON");
            wideFind.Tag = "B30D92054F2BF398";

            var fib = new Fibaro("2", "130.240.74.55", "your_mqtt_username", "your_mqtt_password");
            fib.AddTopic("homeassistant/#");
            

            var client = new AsyncTcpClient();
            //var client = new FileOutput("output.txt");
            var sample = new VelPosSample();

            var controller = new IMotionsController(client,  CancellationToken.None);
            controller.ScheduleExports(new IntervalScheduler(500));
            controller.AddSensor(wideFind);
            controller.AddSample("VelPos", sample);
            controller.AddSampleSensorSubscription("VelPos", "1");
            controller.AddSensor(fib);
            //controller.AddTunnel(1, fib);
         
            client.Connect(new ServerInfo("127.0.0.1", 8089), CancellationToken.None).Wait();
            
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
           

        }

     
  

    }
}
