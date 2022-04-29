using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using iMotionsImportTools.ImportFunctions;
using Serilog;

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

        public ApiService(string baseUrl, int maxConcurrentConnections = DefaultMaxConcurrentConnections, int circuitResetTimeMillis = 15000)
        {
            _client = new HttpClient();
            _circuitBreaker = new CircuitBreaker(maxConcurrentConnections, circuitResetTimeMillis);
            MaxConcurrentConnections = maxConcurrentConnections;
            SetMaxConcurrency(baseUrl, MaxConcurrentConnections);
            _baseUrl = baseUrl;
        }

        public void AddDefaultHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }

        // avoid opening too many sockets, relevant on failed requests
        private void SetMaxConcurrency(string url, int max)
        {
            ServicePointManager.FindServicePoint(new Uri(url)).ConnectionLimit = max; 
        }

        public CircuitBreaker CircuitBreaker()
        {
            return _circuitBreaker;
        }

        public void SetTimeout(int millis)
        {
            _client.Timeout = TimeSpan.FromMilliseconds(millis);
        }

        public int GetTimeout()
        {
            return _client.Timeout.Milliseconds;
        }

        public async Task<string> MakeRequest(string route)
        {
            try
            {
                // to avoid making too many concurrent requests
                await _circuitBreaker.Sem.WaitAsync();


                if (_circuitBreaker.IsOpen())
                {
                    Log.Logger.Warning("Circuit is open for request to: '{A}'", route);
                    return ServiceUnavailable;
                }

                // make the request
                var response = await _client.GetAsync(_baseUrl + route);

                // for requests that are invalid
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Log.Logger.Warning("Data '{A}' does not seem to exist. Multiple instances of this message indicates that this is a faulty request.", route);
                    }
                    
                    _circuitBreaker.OpenCircuit("Status code error: " + response.StatusCode); // open circuit, will force program to wait before making more requests for the unavailable data
                    return ServiceUnavailable;
                }

                return await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex) when (ex is OperationCanceledException || ex is TaskCanceledException || ex is HttpRequestException)
            {
                Log.Logger.Warning("Unexpected error when making request '{A}'. Request failed with error: '{B}'.", route, ex.ToString());
                _circuitBreaker.OpenCircuit();
                return ServiceUnavailable;
            }
            finally
            {
                _circuitBreaker.Sem.Release(); // when the request is completed, allow the next queued request to execute
            }


        }
    }
}
