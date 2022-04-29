using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.iMotionsProtocol
{
    public abstract class WideFindSample : Sample
    {
        public const int VelXIndex = 5;
        public const int VelYIndex = 6;
        public const int VelZIndex = 7;
        public const int PosXIndex = 2;
        public const int PosYIndex = 3;
        public const int PosZIndex = 4;
        public const int IdIndex = 0;

        protected WideFindSample(string sampleType) : base(sampleType)
        {
            ParentSource = "WideFind";
        }

    }
}