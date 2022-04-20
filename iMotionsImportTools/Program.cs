using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.ImportFunctions;
using iMotionsImportTools.Network;
using iMotionsImportTools.Scheduling;
using iMotionsImportTools.Sensor;

using iMotionsImportTools.Controllers;

namespace iMotionsImportTools
{
    class Program
    {
        
        static void Main(string[] args)
        {

            /**var widefind = new WideFind("1", "130.240.74.55");

            if (widefind.Connect())
            {
                widefind.AddTopic("ltu-system/#");
                widefind.AddType("BEACON");
                widefind.Start();
            }

            while (true)
            {
                
                Thread.Sleep(500);
            }
            **/

            /**interval = 500;
            IScheduler scheduler = new IntervalScheduler(interval);
            scheduler.Events += print;

            watch = Stopwatch.StartNew();
            //Console.WriteLine(Stopwatch.IsHighResolution);
            scheduler.Start();
            //var iter = TimeGenerator.IntervalTimEnumerator(500);
            while (true)
            {
                Thread.Sleep(100000);
                scheduler.Stop();
                Console.WriteLine("Max diff: " + max);
                Console.ReadKey();
                //Console.WriteLine(iter.Current);
                //Thread.Sleep((int)iter.Current);
            }**/

            /**var service = new ApiService("http://localhost:8585", 4);
            service.AddDefaultHeader("Authorization", "Basic dW5pY29ybkBsdHUuc2U6alNDTjQ3YkM=");
            for (int i = 0; i < 15; i++)
            {
                var i1 = i;
                Task.Run(async () =>
                {
                    Console.WriteLine("Making request");
                    Console.WriteLine(await service.MakeRequest("/" + i1));
                });
            }

            Console.ReadKey();

            for (int i = 0; i < 15; i++)
            {
                var i1 = i;
                Task.Run(async () =>
                {
                    Console.WriteLine("Making request");
                    Console.WriteLine(await service.MakeRequest("/"+i1));
                });
            }
            Console.ReadKey();*/
            //string regex = @"REPORT:[\w]{16},[\d].[\d].[\d],((-)?[0-9]+,){3}((-)?[0-9]+.[0-9]{2},){3}((-)?\d+.\d\d,){2}(\d+\*(\w)+)";
            //string str = "REPORT:F15DJKL8MNB1ACX1,0.2.7,4500,-397,385,44.01,50.12,100.A43,40.08,-86.05,15745*UJG4D";
            //string regex = @"((-)?[0-9]+.[0-9]{2},)";
            //string str = "40.44,";
            //Console.WriteLine(Regex.Match(str, regex).Success);
            
            /**var preemptive = new PreemptiveScheduler(250, 100);
            ApiService service = new ApiService("https://gorest.co.in");
            ScheduledApiSensor sensor = new ScheduledApiSensor(service, preemptive);
            sensor.AddRoute("/public/v2/comments/1391");
            var client = new AsyncTcpClient();
            client.Connect(new ServerInfo("127.0.0.1", 5678), CancellationToken.None).Wait();
            var task = Task.Run(async () =>
            {
                await client.Receive(CancellationToken.None);
            });
            Controller ctrlr = new Controller(client);
            preemptive.ControllerEvents += ctrlr.ExportAll;
            ctrlr.AddExportable("1", sensor);
            sensor.Start();
            
            Console.ReadKey();
            preemptive.Stop();
            long time = preemptive.fullTest.ElapsedMilliseconds;
            int cycles = preemptive.Cycles;
            double frequency = (double) cycles / time * 1000;
            Console.WriteLine("Cycles: " + cycles + ", Time: " + time + "ms, Frequency: " + frequency + ", Drift: " + preemptive.Drift);
            Console.ReadKey();**/

            double.Parse("-20.45", new CultureInfo("en-US"));
        }

        public static void print_bytes(object sender, byte[] buffer)
        {
            string msg = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.WriteLine("Length: " + msg.Length);
            Console.WriteLine(msg);
        }

    }
}
