using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.Network;
using Serilog;
using tests.Mocks;

namespace tests.IntegrationTests
{
    [TestClass]
    public class ApiServiceTests
    {
        private string _testApiUrl = "http://localhost:8888/";
        private string _testResource = "resource";

        private void ApiResult(bool succeeded, string reason)
        {
            if (!succeeded)
            {
                Console.WriteLine($"Failed with reason: '{reason}'");
                return;
            }
            Console.WriteLine($"Succeeded with message: '{reason}'");
        }

        private void StartTestApi(string expectedData)
        {
            var api = new MockApi(_testApiUrl, _testResource, ApiResult);
            api.Data = expectedData;
            api.StartApi();

        }

        [TestMethod]
        public void TestApiServiceRequest()
        {
            string expectedData = "12";

            var service = new ApiService(_testApiUrl);

            StartTestApi(expectedData);

            var result = service.MakeRequest(_testResource).Result;

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void TestApiServiceCircuitBreakerOpening()
        {
            
            string deadUrl = "http://localhost:1234";
            int numRequests = 5;

            var service = new ApiService(deadUrl);
            bool initialState = service.CircuitBreaker().IsOpen();

            var tasks = new List<Task>();
            for (int i = 0; i < numRequests; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    string result = await service.MakeRequest("");
                    Console.WriteLine(result);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            bool finalState = service.CircuitBreaker().IsOpen();

            Assert.IsFalse( initialState);
            Assert.IsTrue( finalState);
        }

        [TestMethod]
        public void TestApiServiceCircuitBreakerClosing()
        {
            int resetTime = 1; // arbitrary number, time after circuit opening before it closes again
            int leeway = 50; // arbitrary time to allow for request to start
            string deadUrl = "http://localhost:1234";

            var service = new ApiService(deadUrl, circuitResetTimeMillis:resetTime);
            service.SetTimeout(100);
            service.MakeRequest("").Wait();

            Thread.Sleep(service.GetTimeout()+ leeway);

            Assert.IsFalse(service.CircuitBreaker().IsOpen());

        }
    }
}
