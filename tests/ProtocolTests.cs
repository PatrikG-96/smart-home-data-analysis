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

    }
}
