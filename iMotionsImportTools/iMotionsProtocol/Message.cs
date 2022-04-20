using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotionsImportTools.iMotionsProtocol
{
    public class Message
    {

        public static readonly string Event = "E";
        public static readonly string DiscreteMarker = "M";
        public static readonly string DefaultVersion = "1";

        private string _type;

        public string Type {
            get => _type;
            set
            {
                if (value != Event && value != DiscreteMarker)
                {
                    throw new Exception();
                }
                _type = value;
            } 
        }

        public string Version { get; set; } 

        public string Source { get; set; } 

        public string SourceDefinitionVersion { get; set; } = "";

        public string Instance { set; get; } = "";

        public string ElapsedTime { get; set; } = "";

        public string MediaTime { get; set; } = "";

        public Sample Sample { get; set; } = null;
    
        public bool IsValid()
        {
            return Type != null && Version != null && Source != null && Sample != null;
        }

        public override string ToString()
        {
            return $"{Type};{Version};{Source};{SourceDefinitionVersion};{Instance};{ElapsedTime};{MediaTime};{Sample}\r\n";
        }
        
    }
}
