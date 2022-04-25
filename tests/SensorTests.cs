using Microsoft.VisualStudio.TestTools.UnitTesting;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Globalization;
using System.Text;
using iMotionsImportTools.Sensor;

namespace tests
{
    [TestClass]
    public class SensorTests
    {
        [TestMethod]
        public void WideFindParsingTest()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US"); 
            const string tag = "F15B89LAMN134V5H";
            string msg = $"REPORT:{tag},0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";
            string json = $"{{\"message\" : \"{msg}\"," +
                         $"\"source\":\"None\"," +
                         $"\"type\":\"None\"," +
                         $"\"host\":\"None\"," +
                         $"\"time\":\"None\"," +
                         $"\"host\":\"None\"}}";
            var args = new MqttMsgPublishEventArgs("", Encoding.UTF8.GetBytes(json), false, 0, false);

            var sensor = new WideFind("1", "");
            sensor.AddType("REPORT");
            sensor.Tag = tag;
            sensor.OnMessage(null, args);

            Assert.AreEqual(msg, sensor.GetData());
        }
    }
}