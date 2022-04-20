using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.Network;

namespace tests
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void TestTunneler()
        {

            

        }
    }

    class MockClient : IClient
    {
        public Task Send(string data)
        {
            return new Task<string>(() => data);
        }

        public Task Send(string data, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Send(byte[] data, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Connect(ServerInfo info, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task Receive(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
