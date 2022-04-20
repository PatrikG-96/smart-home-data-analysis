using System;
using System.Globalization;
using iMotionsImportTools.Exports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class ExportsTests
    {

        [TestMethod]
        public void TestWideFindFormatVerification()
        {
            // More examples needed
            string valid = "REPORT:F15B89LAMN134V5H,0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";
            string invalid = "REPORT:F15B89LAMN134V5H,0.2.7,45f00,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";

            Assert.IsTrue(WideFindReportData.Verify(valid));
            Assert.IsFalse(WideFindReportData.Verify(invalid));
        }

        [TestMethod]
        public void TestWideFindStringParsing()
        {

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            string messageString = "REPORT:F15B89LAMN134V5H,0.2.7,4500,0,1340,-20.45,10.94,1.00,4.09,13.12,1556123*U6DF";

            var parsedData = (WideFindReportData) WideFindReportData.FromString(messageString);
            var expectedData = new WideFindReportData()
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
            
            Console.WriteLine(parsedData.ToString());
            Console.WriteLine(expectedData.ToString());



            Assert.IsTrue(expectedData.Equals(parsedData));
        }
        
    }
}