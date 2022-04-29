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
using iMotionsImportTools.CLI;
using iMotionsImportTools.Controller;

using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
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
                .MinimumLevel.Error()
                .WriteTo.Console()
                .WriteTo.File("my_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var wideFind = new WideFind("1", "130.240.74.55");
            wideFind.AddTopic("ltu-system/#");
            wideFind.AddType(WideFind.REPORT);
            //wideFind.AddType("BEACON");
            wideFind.Tag = "B30D92054F2BF398";

            var fib = new FibaroSensor("2", "130.240.74.55", "your_mqtt_username", "your_mqtt_password");
            fib.AddDevice(FibaroDevices.U121_DOOR_LIGHT);
            fib.AddDevice(FibaroDevices.U121_DOOR_HUMIDITY);
            fib.AddDevice(FibaroDevices.U121_DOOR_MOTION);
            fib.AddDevice(FibaroDevices.U121_DOOR_TEMP);


            var client = new AsyncTcpClient();
            //var client = new FileOutput("output.txt");
            var stdout = new Stdout();
            var sample = new PositionSample();
            var sample2 = new VelocitySample();
            var sample3 = new FibaroEntranceSample();
            var composite = new PosxAndDoorTemp();

            var controller = new IMotionsController(stdout,  CancellationToken.None);
            controller.ScheduleExports(new IntervalScheduler(500));
            controller.AddSensor(wideFind);
            //controller.AddSensor(fib);
            //controller.AddSample("Pos", sample);
            //controller.AddSample("Vel", sample2);
            //controller.AddSample("Fib", sample3);
            //controller.AddSample("composite", composite);
            //controller.AddSampleSensorSubscription("Pos", "1");
            //controller.AddSampleSensorSubscription("Vel", "1");
            //controller.AddSampleSensorSubscription("Fib", "2");
            //controller.AddSampleSensorSubscription("composite", "1");
            //controller.AddSampleSensorSubscription("composite", "2");
            //controller.AddSensor(fib);
            controller.AddTunnel(1, wideFind);

            client.Connect(new ServerInfo("127.0.0.1", 8089), CancellationToken.None).Wait();
            
            Task.Run(async () =>
            {
                await client.Receive(CancellationToken.None);
            });
            controller.ConnectAll();
            controller.StartAll();

            var cli = new Cli(controller);
            cli.Start();

        }

     
  

    }
}
