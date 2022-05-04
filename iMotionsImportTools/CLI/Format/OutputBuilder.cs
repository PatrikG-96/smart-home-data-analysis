using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.CLI
{
    public class OutputBuilder
    {
        public const int DEFAULT_LINE_LENGTH = 40;
        public int MinLineLength { get; set; } = DEFAULT_LINE_LENGTH;

        public Style ActiveStyle { get; set; } = Style.DefaultStyle();

        //private List<Attribute> _attributes;
        //private readonly int _lineLength;

        private readonly List<Attribute> _attributes;
        private int longestAttributeString;
        private int lineLength;

        private string output;

        public OutputBuilder()
        {
            _attributes = new List<Attribute>();
            longestAttributeString = 0;
            lineLength = 60;
        }

        public void AddTitle(string name)
        {
            longestAttributeString = name.Length > longestAttributeString ? name.Length : longestAttributeString;
            lineLength = Math.Max(longestAttributeString + ActiveStyle.MinValueSpace + (ActiveStyle.Boxed ? 2 : 0), MinLineLength);
            _attributes.Add(new Attribute
            {
                IsTitle = true,
                Key = name
            });
        }

        public void AddAttribute(string name)
        {
            longestAttributeString = name.Length > longestAttributeString ? name.Length : longestAttributeString;
            lineLength = Math.Max(longestAttributeString + ActiveStyle.MinValueSpace + (ActiveStyle.Boxed ? 2 : 0), MinLineLength);
            _attributes.Add(new Attribute
            {
                Key = name
            });
        }


        public void BindValue(string attrName, string attrValue)
        {
            foreach (var attribute in _attributes)
            {
                if (attribute.Key == attrName)
                {
                    attribute.Value = attrValue;
                    return;
                }
            }
        }

        public string Build()
        {
            foreach (var attribute in _attributes)
            {
                if (attribute.IsTitle)
                {
                    BuildTitle(attribute);
                }
                else
                {
                    BuildAttribute(attribute);
                }
            }
            var result = output + Formatter.MakeLine(ActiveStyle.LineEnds, ActiveStyle.LinePad, lineLength);
            output = "";
            return result;
        }

        public void Reset()
        {
            foreach (var attribute in _attributes)
            {
                attribute.Value = null;
            }
        }

        private void BuildTitle(Attribute attr)
        {
            if (!attr.IsTitle) return;
            
            if (ActiveStyle.BoxedTitle)
            {
                output += Formatter.MakeLine(ActiveStyle.LineEnds, ActiveStyle.LinePad, lineLength) + "\n";
                output += Formatter.PadAndCenter(attr.Value, ActiveStyle.TitlePad, ActiveStyle.Edge, lineLength) + "\n";
                output += Formatter.MakeLine(ActiveStyle.LineEnds, ActiveStyle.LinePad, lineLength) + "\n";
            }
            else
            {
                output += Formatter.PadAndCenter(attr.Value, ActiveStyle.TitlePad, ActiveStyle.Edge, lineLength);
            }
        }

        private void BuildAttribute(Attribute attr)
        {
            if (attr.Value == null) return;
            string attrString = "";
            switch (ActiveStyle.KeyAlign)
            {
                case Style.LEFT:
                    
                    if (ActiveStyle.KeyValueDelimiterAlign == Style.LONGEST_KEY_MATCH)
                    {
                        attrString += ActiveStyle.Edge + Formatter.PadAndLeftAlign(attr.Key, ActiveStyle.AttributePad,
                            longestAttributeString) + ActiveStyle.AttributePad + ActiveStyle.KeyValueDelimiter + ActiveStyle.AttributePad; 
                    }
                    
                    switch (ActiveStyle.ValueAlign)
                    {
                        case Style.INLINE:

                            attrString += Formatter.PadAndLeftAlign(ValueWrap(attrString, attr.Value),' ', lineLength-attrString.Length-1) + (ActiveStyle.Boxed ? new string(ActiveStyle.Edge, 1) : "");

                            break;
                        case Style.LEFT:
                            break;
                        case Style.RIGHT:
                            break;
                        default:
                            throw new Exception("Value Align error");
                    }
                    
                    break;
                case Style.CENTER:
                    break;
                case Style.RIGHT:
                    break;
            }


            output += attrString + "\n";


        }

        private string ValueWrap(string attrString, string value, bool wrapping = false)
        {
            if (attrString.Length + value.Length > lineLength - (ActiveStyle.Boxed ? 1 : 0) && ActiveStyle.ValueWrap)
            {

                switch (ActiveStyle.ValueWrapStrategy)
                {
                    case Style.INLINE:

                        int currentLength = attrString.Length;

                        int valueSpace = lineLength - currentLength - (ActiveStyle.Boxed ? 1 : 0);

                        string valueInRow = value.Substring(0, valueSpace) + (ActiveStyle.Boxed ? new string(ActiveStyle.Edge,1) : "");

                        if (wrapping) valueInRow = new string(ActiveStyle.Edge, 1) + valueInRow;

                        return valueInRow + "\n" + ValueWrap((ActiveStyle.Boxed ? new string(ActiveStyle.Edge, 1) : ""),
                            value.Substring(valueSpace), true);

                       
                    case Style.BELOW:
                        return "";
                      
                }

            }
            if (wrapping)
                return attrString + Formatter.PadAndRightAlign(value, ActiveStyle.AttributePad, lineLength - (attrString.Length+1));
            return value;
        }


        public static void StandardSensorOutput(ISensor sensor, OutputBuilder builder)
        {
            if (sensor is WideFind wideFind)
            {
                builder.BindValue("title", wideFind.GetType().Name);
                builder.BindValue("ID", wideFind.Id);
                builder.BindValue("Host", wideFind.Broker);
                builder.BindValue("Tag", wideFind.Tag);

            }
            else if (sensor is FibaroSensor fib)
            {
                builder.BindValue("title", fib.GetType().Name);
                builder.BindValue("ID", fib.Id);
                builder.BindValue("Host", fib.Broker);

                var devices = fib.GetDeviceIds();

                builder.BindValue("Devices", string.Join(", ", devices));
            }
        }
    }
}