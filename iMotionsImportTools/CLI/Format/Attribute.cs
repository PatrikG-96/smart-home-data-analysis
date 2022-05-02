using System;
using System.Runtime.Versioning;

namespace iMotionsImportTools.CLI
{
    public class Attribute
    {
        public bool IsTitle { get; set; }
        public string Key { get; set; }
        public string Value { get; set; } = null;
    }
}