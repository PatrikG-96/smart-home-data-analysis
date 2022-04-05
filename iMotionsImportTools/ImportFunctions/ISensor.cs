using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotionsImportTools.ImportFunctions
{
    public interface ISensor
    {

        bool Connect(string address, int port);

        bool Disconnect();

        void Start();

        void Stop();

    }
}
