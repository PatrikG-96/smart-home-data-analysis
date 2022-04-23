using System.Threading.Tasks;

namespace iMotionsImportTools.Output
{
    public interface IOutputDevice
    {

        Task Write(string message);

    }
}