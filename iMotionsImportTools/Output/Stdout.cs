using System;
using System.Threading.Tasks;

namespace iMotionsImportTools.Output
{
    public class Stdout: IOutputDevice
    {
        public void Write(string message)
        {
            Task.Run( async () =>
            {
                await Console.Out.WriteLineAsync(message);
            });
        }
    }
}