using iMotionsImportTools.iMotionsProtocol;

namespace iMotionsImportTools.Protocols.H2AlProtocol
{
    public class H2AlProtocol : IProtocol
    {
        public string SampleToMessage(Sample sample, long timestamp)
        {

            if (sample is VelocitySample vel)
            {
                return $"{vel.SampleType};{timestamp};{vel.VelX};{vel.VelY};{vel.VelZ}";
            }
            if (sample is PositionSample pos)
            {
                return $"{pos.SampleType};{timestamp};{pos.PosX};{pos.PosY};{pos.PosZ}";
            }

            return null;

        }
    }
}