using System.Globalization;
using System.Text;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace tests.UnitTests
{
    [TestClass]
    public class SampleTests
    {

        private WideFind InitializeTestSensor(WideFindMessage message)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            const string tag = "F15B89LAMN134V5H";
            string msg = $"REPORT:{tag},0.2.7,{message.PosX},{message.PosY},{message.PosZ},{message.VelX},{message.VelY},{message.VelZ},4.09,13.12,1556123*U6DF";
            string json = $"{{\"message\" : \"{msg}\"," +
                          $"\"source\":\"None\"," +
                          $"\"type\":\"None\"," +
                          $"\"host\":\"None\"," +
                          $"\"time\":\"None\"," +
                          $"\"host\":\"None\"}}";
            var args = new MqttMsgPublishEventArgs("", Encoding.UTF8.GetBytes(json), false, 0, false);

            var sensor = new WideFind("1", "");
            sensor.AddType(WideFind.REPORT);
            sensor.AddTopic("test");
            sensor.Tag = tag;
            sensor.Start();
            sensor.OnMessage(null, args);

            return sensor;
        }

        [TestMethod]
        public void TestWideFindPositionSample()
        {
            const string posX = "4500";
            const string posY = "0";
            const string posZ = "1340";

            var sensor = InitializeTestSensor(new WideFindMessage
            {
                PosX = posX,
                PosY = posY,
                PosZ = posZ
            });

            var posSampleExpected = new PositionSample
            {
                PosX = posX,
                PosY = posY,
                PosZ = posZ
            };

            
            var resultingPosSample = new PositionSample();

            resultingPosSample.InsertSensorData(sensor);

            
            Assert.AreEqual(posSampleExpected.ToString(), resultingPosSample.ToString());
        }

        [TestMethod]
        public void TestWideFindVelocitySample()
        {

            const string velX = "-20.45";
            const string velY = "10.94";
            const string velZ = "1.00";

            var sensor = InitializeTestSensor(new WideFindMessage
            {
                VelX = velX,
                VelY = velY,
                VelZ = velZ
            });

            var velSampleExpected = new VelocitySample
            {
                VelX = velX,
                VelY = velY,
                VelZ = velZ
            };

            var resultingVelSample = new VelocitySample();
            resultingVelSample.InsertSensorData(sensor);

            Assert.AreEqual(velSampleExpected.ToString(), resultingVelSample.ToString());
        }

        
    }
}