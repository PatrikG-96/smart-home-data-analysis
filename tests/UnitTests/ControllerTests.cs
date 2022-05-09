using System;
using System.Linq;
using System.Threading;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tests.Mocks;

namespace tests.UnitTests
{
    [TestClass]
    public class ControllerTests
    {

        [TestMethod]
        public void TestSampleExporting()
        {
            int n = 10;
            int Min = 0;
            int Max = 20;
            Random randNum = new Random();
            int[] outputArray = Enumerable
                .Repeat(0, n)
                .Select(i => randNum.Next(Min, Max))
                .ToArray();

            string[] expected = new string[n];

            for (int i = 0; i < n; i++)
            {
                var sampleExample = new MockSample
                {
                    Attribute = outputArray[i]
                };
                var msg = new Message()
                {
                    Source = sampleExample.ParentSource,
                    Version = Message.DefaultVersion,
                    Type = Message.Event,
                    Sample = sampleExample.Copy() // to avoid race conditions when resetting samples
                };
                expected[i] = msg.ToString();
            }

            var sensor = new MockSensor(outputArray)
            {
                Id = "1"
            };
            var sample = new MockSample();
            var output = new MockOutputDevice(n);

            var controller = new SensorController(output, CancellationToken.None);
            controller.ScheduleExports(new MockScheduler(n));
            controller.AddSensor(sensor);
            controller.AddSample("mock", sample);
            controller.AddSampleSensorSubscription("mock", "1");
            controller.ConnectAll();
            controller.StartAll();

            Assert.IsTrue(expected.SequenceEqual(output.Data));
        }
        
    }
}