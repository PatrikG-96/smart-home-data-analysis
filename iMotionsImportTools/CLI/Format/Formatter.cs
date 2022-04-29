namespace iMotionsImportTools.CLI
{
    public static class Formatter
    {
        public const int NONE = -2;
        public const int CENTER = 0;
        public const int RIGHT = -1;
        public const int LEFT = 1;

        public static string PadAndCenter(string target, char pad, int lineLength)
        {
            if (target.Length >= lineLength)
            {
                return target;
            }

            int leftPadding = (lineLength - target.Length) / 2;
            int rightPadding = lineLength - target.Length - leftPadding;

            return new string(pad, leftPadding) + target + new string(pad, rightPadding);
        }
        public static string PadAndCenter(string target, char pad, char edge, int lineLength)
        {
            lineLength -= 2;
            if (target.Length >= lineLength)
            {
                return target;
            }

            int leftPadding = (lineLength - target.Length) / 2;
            int rightPadding = lineLength - target.Length - leftPadding;

            return new string(edge, 1) + new string(pad, leftPadding) + target + new string(pad, rightPadding) +new string(edge, 1);
        }

        public static string Repeat(char character, int n)
        {
            return new string(character, n);
        }

        public static string PadAndLeftAlign(string target, char pad, int lineLength)
        {
            if (target.Length >= lineLength)
            {
                return target;
            }

            int rightPadding = lineLength - target.Length;

            return target + new string(pad, rightPadding);
        }

        public static string PadAndRightAlign(string target, char pad, int lineLength)
        {
            if (target.Length >= lineLength)
            {
                return target;
            }

            int leftPadding = lineLength - target.Length;

            return new string(pad, leftPadding) + target;
        }

        public static string MakeLine(char edges, char padding, int n)
        {
            return new string(edges, 1) + new string(padding, n - 2) + new string(edges, 1);
        }
    }
}