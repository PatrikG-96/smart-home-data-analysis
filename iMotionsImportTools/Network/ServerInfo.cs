using System;
using System.Net;

namespace iMotionsImportTools.Network
{
    public class ServerInfo
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public ServerInfo(string address, int port)
        {
            Address = address;
            Port = port;
        }
    }
}