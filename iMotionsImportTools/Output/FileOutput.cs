using System.IO;
using System.Threading.Tasks;
using iMotionsImportTools.Protocols;

namespace iMotionsImportTools.Output
{
    public class FileOutput: IOutputDevice
    {

        private readonly string _filename;

        public FileOutput(string filename)
        {
            _filename = filename;
        }

        public string Id { get; set; }

        public void Write(string message)
        {
            Task.Run(async () =>
            {
                using (StreamWriter sw = File.AppendText(_filename))
                {
                    await sw.WriteAsync(message);
                }
            });

        }
    }
}