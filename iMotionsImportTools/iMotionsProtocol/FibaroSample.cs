namespace iMotionsImportTools.iMotionsProtocol
{
    public abstract class FibaroSample : Sample
    {
        protected FibaroSample(string sampleType) : base(sampleType)
        {
            ParentSource = "Fibaro";
        }
    }
}