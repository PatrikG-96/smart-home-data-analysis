using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
using iMotionsImportTools.Protocols.H2AlProtocol;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
using Newtonsoft.Json;
using Serilog;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace iMotionsImportTools
{
    
    class Program
    {
        
        static void Main(string[] args)
        {




            //Console.ReadKey();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Console()
                .WriteTo.File("my_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var wideFind = new WideFind("1", "130.240.74.55");
            wideFind.AddTopic("ltu-system/#");
            wideFind.AddType(WideFind.REPORT);
            //wideFind.AddType("BEACON");
            //wideFind.Tag = "B30D92054F2BF398";
            wideFind.Tag = "F1587D88122BE247";

            //var fib = new FibaroSensor("2", "130.240.74.55", "your_mqtt_username", "your_mqtt_password");
            //fib.AddDevice(FibaroDevices.U121_DOOR_LIGHT);
            //fib.AddDevice(FibaroDevices.U121_DOOR_HUMIDITY);
            //fib.AddDevice(FibaroDevices.U121_DOOR_MOTION);
            //fib.AddDevice(FibaroDevices.U121_DOOR_TEMP);


            var client = new AsyncTcpClient();
            client.Connect(new ServerInfo("127.0.0.1", 8089), CancellationToken.None).Wait();
            

            Task.Run(async () =>
            {
                await client.Receive(CancellationToken.None);
            });

            Task.Run(async () =>
            {
                await client.Start(CancellationToken.None);
            });


            //var client = new FileOutput("output.txt");
            var stdout = new Stdout();
            var sample = new PositionSample();
            var sample2 = new VelocitySample
            {
                Instance = "Synchronized"
            };
            var sample3 = new EntranceSample();
            var controller = new SensorController(client, new IMotionsProtocol());
            client.OnDisconnect += controller.OnDisconnect;

            controller.SetExportScheduler(new IntervalScheduler(100));
            controller.AddSensor(wideFind);
            //controller.AddSensor(fib);
            //controller.AddSample("Pos", sample);
            controller.AddSample("Vel", sample2);
            //controller.AddSample("Fib", sample3);
            //controller.AddSample("composite", composite);
            //controller.AddSampleSensorSubscription("Pos", "1");
            controller.AddSampleSensorSubscription("Vel", "1");
            //controller.AddSampleSensorSubscription("Fib", "2");
            //controller.AddSampleSensorSubscription("composite", "1");
            //controller.AddSampleSensorSubscription("composite", "2");
            //controller.AddSensor(fib);
            //controller.AddTunnel(1, wideFind);

            
            
            controller.ConnectAll();
            controller.StartAll();
            
            //var cli = new Cli(controller);
            //cli.Start();
            Console.ReadKey();

            controller.Quit();

            Console.ReadKey();
        }



    }
}
