using System.Threading.Tasks;

namespace iMotionsImportTools.Output
{
    public interface IOutputDevice
    {

        string Id { get; set; }
        void Write(string message);

    }
}