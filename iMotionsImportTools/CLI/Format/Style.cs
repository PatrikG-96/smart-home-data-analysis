using System;

namespace iMotionsImportTools.CLI
{
    public class Style
    {

        public const int LEFT = 0;
        public const int RIGHT = 1;
        public const int CENTER = 2;
        public const int INLINE = 3;
        public const int LONGEST_KEY_MATCH = 4;
        public const int BELOW = 5;

        private int titleAlign;
        private int keyAlign;
        private int keyValueDelimAlign;
        private int valueAlign;
        private int valueWrapStrategy;

        public char KeyValueDelimiter { get; set; }

        public char Edge { get; set; }

        public char LineEnds { get; set; }

        public char LinePad { get; set; }

        public char TitlePad { get; set; }

        public char AttributePad { get; set; }

        public bool BoxedTitle { get; set; }

        public bool Boxed { get; set; }

        public bool ValueWrap { get; set; }

        public int MinValueSpace { get; set; }
        public int TitleAlign
        {
            get => titleAlign;
            set
            {
                if (!(value == LEFT || value == RIGHT || value == CENTER)) throw new Exception("Invalid alignment");
                titleAlign = value;
            }
        }
        public int KeyAlign
        {
            get => keyAlign;
            set
            {
                if (!(value == LEFT || value == RIGHT || value == CENTER)) throw new Exception("Invalid alignment");
                titleAlign = value;
            }
        }

        public int ValueAlign
        {
            get => valueAlign;
            set
            {
                if (!(value == LEFT || value == RIGHT || value == CENTER || value == INLINE)) throw new Exception("Invalid alignment");
                titleAlign = value;
            }
        }

        public int KeyValueDelimiterAlign
        {
            get => keyValueDelimAlign;
            set
            {
                if (!(value == INLINE || value == CENTER || value == LONGEST_KEY_MATCH)) throw new Exception("Invalid alignment");
                titleAlign = value;
            }
        }

        public int ValueWrapStrategy
        {
            get => valueWrapStrategy;
            set
            {
                if (!(value == INLINE || value == BELOW)) throw new Exception("Invalid alignment");
                titleAlign = value;
            }
        }

        public static Style DefaultStyle()
        {
            return new Style
            {
                Boxed = true,
                BoxedTitle = true,
                Edge = '|',
                LineEnds = '+',
                LinePad = '-',
                TitleAlign = CENTER,
                KeyAlign = LEFT,
                KeyValueDelimiter = ':',
                KeyValueDelimiterAlign = LONGEST_KEY_MATCH,
                ValueAlign = INLINE,
                ValueWrap = true,
                ValueWrapStrategy = INLINE,
                MinValueSpace = 10,
                TitlePad = ' ',
                AttributePad = ' '
            };
        }

    }
}