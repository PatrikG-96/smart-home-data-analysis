using System;
using System.Collections.Generic;
using System.Globalization;
using iMotionsImportTools.Exports;
using iMotionsImportTools.iMotionsProtocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class ExportsTests
    {
        [TestMethod]
        public void TestProtocolMessageParsing()
        {
            string expected = "E;1;WideFind;;;;;VelPos;F15B89LAMN134V5H;-20.34;10.34;1.00;4500;3100;100\r\n";
        
            var msg = new Message
            {
                Source = "WideFind",
                Type = Message.Event,
                Version = "1"
            };

            var sample = new OldSample("VelPos", new List<string> { "F15B89LAMN134V5H","-20.34", "10.34", "1.00", "4500", "3100","100"});
            msg.Sample = sample;

     
            Assert.AreEqual(msg.ToString(), expected);

        }

        [TestMethod]
        public void TestWideFindFormatVerification()
        {
            // More examples needed
            string valid = "REPORT:F15B89LAMN134V5H,0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";
            string invalid = "REPORT:F15B89LAMN134V5H,0.2.7,45f00,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";

            Assert.IsTrue(WideFindReport.Verify(valid));
            Assert.IsFalse(WideFindReport.Verify(invalid));
        }

        [TestMethod]
        public void TestWideFindStringParsing()
        {

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            string messageString = "REPORT:F15B89LAMN134V5H,0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";

            var parsedData = (WideFindReport) WideFindReport.FromString(messageString);

            var expectedData = new WideFindReport()
            {
                Address = "F15B89LAMN134V5H",
                Version = "0.2.7",
                Battery = 4.09,
                PosX = 4500,
                PosY = 0,
                PosZ = 1340,
                VelX = -20.45,
                VelY = 10.94,
                VelZ = 1.00,
                Rssi = 13.12,
                TimeAlive = 1556123
            };

            Assert.IsTrue(expectedData.Equals(parsedData));
        }
        
    }
}