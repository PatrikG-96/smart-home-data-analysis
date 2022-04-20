using System.Collections.Generic;
using System.Xml;
using iMotionsImportTools.Exports;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class FallDetectionDataCollection : IDataCollection
    {

        public double VelX { get; set; }
        public double VelY { get; set; }
        public double VelZ { get; set; }

        IDataCollection IDataCollection.ConstructFromCollection(List<ExportData> dataObjects)
        {
            return FallDetectionDataCollection.ConstructFromCollection(dataObjects);
        }

        
        public static IDataCollection ConstructFromCollection(List<ExportData> dataObjects)
        {
            // Only a single widefind data object is needed/accepted
            if (dataObjects.Count > 1)
            {
                // throw exception
                return null;
            }

            var data = dataObjects[0];

            if (!(data is WideFindReportData wideFindData))
            {
                return null; // throw exception
            }

            var newObject = new FallDetectionDataCollection
            {
                VelX = wideFindData.VelX,
                VelY = wideFindData.VelY,
                VelZ = wideFindData.VelZ
            };

            return newObject;
        }

        public XmlDocument FormatToXml()
        {
            throw new System.NotImplementedException();
        }

    }
}