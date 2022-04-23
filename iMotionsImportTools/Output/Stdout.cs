using System;
using System.Threading.Tasks;

namespace iMotionsImportTools.Output
{
    public class Stdout: IOutputDevice
    {
        public async Task Write(string message)
        {
            await Console.Out.WriteLineAsync(message);
        }
    }
}