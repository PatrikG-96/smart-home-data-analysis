using System;

namespace iMotionsImportTools.Exports
{
    public abstract class ExportData
    {
        public static ExportData FromString(string str)
        {
            throw new NotImplementedException();
        }
        public abstract string StringRepr();

    }
}