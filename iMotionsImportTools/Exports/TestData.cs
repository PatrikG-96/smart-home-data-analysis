namespace iMotionsImportTools.Exports
{
    public class TestData : ExportData
    {

        public string Message { get; set; }

        public override string StringRepr()
        {
            return Message;
        }
    }
}