using iMotionsImportTools.Protocols;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class IMotionsProtocol : IProtocol
    {
        public string SampleToMessage(Sample sample, long timestamp)
        {
            var msg = new Message
            {
                Source = sample.ParentSource,
                Version = Message.DefaultVersion,
                Type = Message.Event,
                Instance = sample.Instance,
                SampleString = sample.ToString()
            };

            return msg.ToString();
        }
    }
}