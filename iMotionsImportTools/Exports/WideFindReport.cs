using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace iMotionsImportTools.Exports
{
    public class WideFindReport : ExportData
    {
        private static readonly string[] _fieldOrder = new[]
            {"address", "version", "posX", "posY", "posZ", "velX", "velY", "velZ", "battery", "rssi", "timealive"};
        private static readonly string _regexPattern =  @"REPORT:[\w]{16},[\d].[\d].[\d],((-)?[0-9]+,){3}((-)?[0-9]+.[0-9]{2},){3}((-)?\d+.\d\d,){2}(\d+\*(\w)+)";
        public string MessageString {get; set;} ="";

        public double VelX { get; set; }
        public double VelY { get; set; }
        public double VelZ { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }
        public string Address { get; set; }
        public long TimeAlive { get; set; }
        public string Version { get; set; }
        public double Battery { get; set; }
        public double Rssi { get; set; }

        public static bool Verify(string msg)
        {
            return Regex.IsMatch(msg, _regexPattern);
        }

        public new static ExportData FromString(string str)
        {
            if (!Verify(str))
            {
                throw new FormatException();
            }

            var colonSeparated = str.Substring(str.IndexOf(':')+1);

            var separatedFields = colonSeparated.Split(',');


            var data = new WideFindReport
            {
                MessageString = str
            };
            for (int i = 0; i < separatedFields.Length; i++)
            {
                var fieldName = _fieldOrder[i];
                var fieldValue = separatedFields[i];
                switch (fieldName)
                {
                    case "address":
                        data.Address = fieldValue;
                        break;
                    case "version":
                        data.Version = fieldValue;
                        break;
                    case "posX":
                        data.PosX = int.Parse(fieldValue);
                        break;
                    case "posY":
                        data.PosY = int.Parse(fieldValue);
                        break;
                    case "posZ":
                        data.PosZ = int.Parse(fieldValue);
                        break;
                    case "velX":
                        data.VelX = double.Parse(fieldValue);
                        break;
                    case "velY":
                        data.VelY = double.Parse(fieldValue);
                        break;
                    case "velZ":
                        data.VelZ = double.Parse(fieldValue);
                        break;
                    case "battery":
                        data.Battery = double.Parse(fieldValue);
                        break;
                    case "rssi":
                        data.Rssi = double.Parse(fieldValue);
                        break;
                    case "timealive":
                        data.TimeAlive = long.Parse(fieldValue.Split(('*'))[0]);
                        break;
                }
            }

            return data;
        }

        public override string StringRepr()
        {
            // Either move or parse to imotions compliant string
            return MessageString;
        }

        public new bool Equals(object o)
        {
            // should handle null?
            if (!(o is WideFindReport))
            {
                return false;
            }
            var other = (WideFindReport)o;
            bool velEqual = VelX == other.VelX && VelY == other.VelY && VelZ == other.VelZ;
            bool posEqual = PosX == other.PosX && PosY == other.PosY && PosZ == other.PosZ;
            bool otherEqual = Address == other.Address && Version == other.Version &&
                              Rssi == other.Rssi && Battery == other.Battery;
            return velEqual && posEqual && otherEqual;
        }



        public new string ToString()
        {
            return
                $"Address:{Address}, Version:{Version}, PosX:{PosX}, PosY:{PosY}, PosZ:{PosZ}, VelX:{VelX}, VelY:{VelY}, VelZ:{VelZ}, Battery:{Battery}, RSSI:{Rssi}, TimeAlive:{TimeAlive}";
        }
    }
}