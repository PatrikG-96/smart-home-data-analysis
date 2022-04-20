using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using iMotionsImportTools.Exports;

namespace iMotionsImportTools.iMotionsProtocol
{
    public interface IDataCollection
    {

        IDataCollection ConstructFromCollection(List<ExportData> dataObjects);
        XmlDocument FormatToXml();

    }
}