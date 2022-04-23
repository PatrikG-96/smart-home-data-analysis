using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using iMotionsImportTools.ImportFunctions;

namespace iMotionsImportTools.Network
{
    public class ApiService
    {
        public int MaxConcurrentConnections { set; get; }

        public const int DefaultMaxConcurrentConnections = 5;

        private readonly string _baseUrl;

        private readonly HttpClient _client;
        private readonly CircuitBreaker _circuitBreaker;

        private const string ServiceUnavailable = "Unavailable.";

        public ApiService(string baseUrl, int maxConcurrentConnections = DefaultMaxConcurrentConnections)
        {
            _client = new HttpClient();
            _circuitBreaker = new CircuitBreaker(maxConcurrentConnections, 15000);
            MaxConcurrentConnections = maxConcurrentConnections;
            SetMaxConcurrency(baseUrl, MaxConcurrentConnections);
            _baseUrl = baseUrl;
        }

        public void AddDefaultHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }

        private void SetMaxConcurrency(string url, int max)
        {
            ServicePointManager.FindServicePoint(new Uri(url)).ConnectionLimit = max;
        }

        public async Task<string> MakeRequest(string route)
        {
            try
            {

                await _circuitBreaker.Sem.WaitAsync();


                if (_circuitBreaker.IsOpen())
                {
                    Console.WriteLine("Circuit is open for request: " + route);
                    return ServiceUnavailable;
                }

                var completeUrl = _baseUrl + route;

                var response = await _client.GetAsync(_baseUrl + route);



                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _circuitBreaker.OpenCircuit("Status code error: " + response.StatusCode);
                    return ServiceUnavailable;
                }

                return await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex) when (ex is OperationCanceledException || ex is TaskCanceledException || ex is HttpRequestException)
            {
                _circuitBreaker.OpenCircuit("Some error occurred for request: " + route);
                return ServiceUnavailable;
            }
            finally
            {
                _circuitBreaker.Sem.Release();
            }


        }
    }
}
