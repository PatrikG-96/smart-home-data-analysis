using System;
using System.Runtime.Versioning;

namespace iMotionsImportTools.CLI
{
    public class Attribute
    {
        public const char SKIP = '\0';
        public string Value { get; set; }
        public int Align { get; set; }
        public int Length { get; set; }
        public char Pad { get; set; }
        public char Edges { get; set; }
        public char Surround { get; set; }

        public override string ToString()
        {
            int edgeOffset = Edges == SKIP ? 0 : 2;
            string stringValue = Surround == SKIP ? Value : new string(Surround, 1) + Value + new string(Surround, 1);
            var result = "";
            switch (Align)
            {
                case Formatter.LEFT:
                    result =  Formatter.PadAndLeftAlign(stringValue, Pad, Length - edgeOffset);
                    break;
                case Formatter.RIGHT:
                    result = Formatter.PadAndRightAlign(stringValue, Pad, Length - edgeOffset);
                    break;
                case Formatter.CENTER:
                    result = Formatter.PadAndCenter(stringValue, Pad, Length - edgeOffset);
                    break;
                case Formatter.NONE:
                    break;
                default:
                    throw new Exception("shit");
            }
            if (Edges != SKIP)
            {
                result = Edges + result + Edges;
            }
            return result;
        }

    }
}