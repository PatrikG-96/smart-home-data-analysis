using System.Threading.Tasks;

namespace iMotionsImportTools.Output
{
    public interface IOutputDevice
    {

        void Write(string message);

    }
}