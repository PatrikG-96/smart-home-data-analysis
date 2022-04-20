using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iMotionsImportTools.Network
{
    public interface IClient
    {
        Task Send(string data);
        Task Send(string data, CancellationToken token);
        Task Send(byte[] data, CancellationToken token);
        Task Connect(ServerInfo info, CancellationToken token);

        Task Receive(CancellationToken token);

    }
}
