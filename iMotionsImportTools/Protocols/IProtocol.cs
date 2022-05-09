using iMotionsImportTools.iMotionsProtocol;

namespace iMotionsImportTools.Protocols
{
    public interface IProtocol
    {

        string SampleToMessage(Sample sample, long timestamp);

    }
}