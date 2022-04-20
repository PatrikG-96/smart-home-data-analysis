using iMotionsImportTools.iMotionsProtocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace tests
{
    [TestClass]
    public class ProtocolTests
    {
        [TestMethod]
        public void TestVelPosSample()
        {

            string wideFindString = "REPORT:F15B89LAMN134V5H,0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";

            var expectedSample = new VelPosSample
            {
                Id = "F15B89LAMN134V5H",
                PosX = "4500",
                PosY = "0",
                PosZ = "1340",
                VelX = "-20.45",
                VelY = "10.94",
                VelZ = "1.00"
            };

            var resultingSample = VelPosSample.FromString(wideFindString);

            Console.WriteLine("Expected: " + expectedSample);
            Console.WriteLine("Resulting: " + resultingSample);

            Assert.IsTrue(VelPosSample.VelPosSampleComparer.Equals(resultingSample, expectedSample));
        }

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

            var sample = new OldSample("VelPos", new List<string> { "F15B89LAMN134V5H", "-20.34", "10.34", "1.00", "4500", "3100", "100" });
            msg.Sample = sample;


            Assert.AreEqual(msg.ToString(), expected);

        }
    }
}
