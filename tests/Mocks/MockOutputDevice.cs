using System.Collections.Generic;
using System.Threading.Tasks;
using iMotionsImportTools.Output;

namespace tests.Mocks
{
    public class MockOutputDevice : IOutputDevice
    {
        public int NumWrites { get; private set; } = 0;
        public string[] Data { get; private set; }

        public MockOutputDevice(int numInputs)
        {
            Data = new string[numInputs];
        }

        public void Write(string message)
        {
            // Not thread safe
            Data[NumWrites] = message;
            NumWrites++;
        }
    }
}