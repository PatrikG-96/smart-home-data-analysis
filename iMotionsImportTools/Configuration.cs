using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.Network;
using iMotionsImportTools.Controller;

namespace iMotionsImportTools
{
    public class Configuration
    {

        public ServerInfo RemoteHost { get; private set; }
        public AsyncTcpClient Client { get; private set; }
        public ExportController Controller { get; private set; }

        public static Configuration Configure()
        {

            var config = new Configuration();

            string remoteHost = "127.0.0.1";
            int remotePort = 8089;

            var info = new ServerInfo(remoteHost, remotePort);
            var client = new AsyncTcpClient();

            Task.Run(async () =>
            {
                client.Connect()
            });

        }

    }
}
