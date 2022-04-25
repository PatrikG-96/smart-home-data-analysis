using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.Sensor;
using Serilog;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class VelPosSample : Sample
    {
        

        public static IEqualityComparer<VelPosSample> VelPosSampleComparer { get; } = new VelPosSampleEqualityComparer();

        private const int VelXIndex = 5;
        private const int VelYIndex = 6;
        private const int VelZIndex = 7;
        private const int PosXIndex = 2;
        private const int PosYIndex = 3;
        private const int PosZIndex = 4;
        private const int IdIndex = 0;


        public string VelX { get; set;}
        public string VelY { get; set; }
        public string VelZ { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string Id { get; set; }


        public VelPosSample() : base("VelPos")
        {
            ParentSource = "WideFind";
        }

        public static VelPosSample FromString(string messageString)
        {
            var colonSeparated = messageString.Substring(messageString.IndexOf(':') + 1);

            var separatedFields = colonSeparated.Split(',');

            var sample = new VelPosSample
            {
                Id = separatedFields[IdIndex],
                VelX = separatedFields[VelXIndex],
                VelY = separatedFields[VelYIndex],
                VelZ = separatedFields[VelZIndex],
                PosX = separatedFields[PosXIndex],
                PosY = separatedFields[PosYIndex],
                PosZ = separatedFields[PosZIndex]
            };
            return sample;
        }

        public override string ToString()
        {
            return $"{SampleType};{Id};{VelZ};{VelY};{VelX};{PosZ};{PosY};{PosX}";
        }

        public override void Reset()
        {
            VelX = VelY = VelZ = PosX = PosY = PosZ = Id = null;
        }

        public override Sample Copy()
        {
            return new VelPosSample
            {
                Id = this.Id,
                PosX = this.PosX,
                PosY = this.PosY,
                PosZ = this.PosZ,
                VelX = this.VelX,
                VelY = this.VelY,
                VelZ = this.VelZ
            };
        }

        public override void InsertSensorData(ISensor sensor)
        {
            if (sensor is WideFind wideFind)
            {
                var messageString = wideFind.GetData();

                if (messageString == null)
                {
                    return;
                }

                var colonSeparated = messageString.Substring(messageString.IndexOf(':') + 1);
                Log.Logger.Debug("Message: {A}", colonSeparated);
                var separatedFields = colonSeparated.Split(',');
                Id = separatedFields[IdIndex];
                VelX = separatedFields[VelXIndex];
                
                VelY = separatedFields[VelYIndex];
                VelZ = separatedFields[VelZIndex];
                PosX = separatedFields[PosXIndex];
                Log.Logger.Debug("PosX: {A}", PosX);
                PosY = separatedFields[PosYIndex];
                PosZ = separatedFields[PosZIndex];
            }
        }

        private sealed class VelPosSampleEqualityComparer : IEqualityComparer<VelPosSample>
        {
            public bool Equals(VelPosSample x, VelPosSample y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.VelX == y.VelX && x.VelY == y.VelY && x.VelZ == y.VelZ && x.PosX == y.PosX && x.PosY == y.PosY && x.PosZ == y.PosZ && x.Id == y.Id;
            }

            public int GetHashCode(VelPosSample obj)
            {
                unchecked
                {
                    var hashCode = (obj.VelX != null ? obj.VelX.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.VelY != null ? obj.VelY.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.VelZ != null ? obj.VelZ.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.PosX != null ? obj.PosX.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.PosY != null ? obj.PosY.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.PosZ != null ? obj.PosZ.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Id != null ? obj.Id.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }


    }
}
