﻿using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace iMotionsImportTools.CLI
{
    public class OutputBuilder
    {

        public Style ActiveStyle { get; set; } = Style.DefaultStyle();

        //private List<Attribute> _attributes;
        //private readonly int _lineLength;

        private readonly List<Attribute> _attributes;
        private int longestAttributeString;
        private int lineLength;

        private string output;

        public OutputBuilder(int lineLength)
        {
            _attributes = new List<Attribute>();
            longestAttributeString = 0;
        }

        public void AddTitle(string name)
        {
            longestAttributeString = name.Length > longestAttributeString ? name.Length : longestAttributeString;
            lineLength = longestAttributeString + ActiveStyle.MinValueSpace + (ActiveStyle.Boxed ? 2 : 0);
            _attributes.Add(new Attribute
            {
                IsTitle = true,
                Key = name
            });
        }

        public void AddAttribute(string name)
        {
            longestAttributeString = name.Length > longestAttributeString ? name.Length : longestAttributeString;
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
                    
                }
            }

            return output;
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
            switch (ActiveStyle.KeyAlign)
            {
                case Style.LEFT:

                    if (ActiveStyle.KeyValueDelimiterAlign == Style.LONGEST_KEY_MATCH)
                    {
                        output += ActiveStyle.Edge + Formatter.PadAndLeftAlign(attr.Key, ActiveStyle.AttributePad,
                            longestAttributeString); 
                    }
                    
                    break;
                case Style.CENTER:
                    break;
                case Style.RIGHT:
                    break;
            }

            
        }

    }
}