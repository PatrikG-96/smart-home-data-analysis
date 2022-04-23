using System.IO;
using System.Threading.Tasks;

namespace iMotionsImportTools.Output
{
    public class FileOutput: IOutputDevice
    {

        private string _filename;

        public FileOutput(string filename)
        {
            _filename = filename;
        }

        public async Task Write(string message)
        {
            using (StreamWriter sw = File.AppendText(_filename))
            {
               await sw.WriteAsync(message);
            }
        }
    }
}