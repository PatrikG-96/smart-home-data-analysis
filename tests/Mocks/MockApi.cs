using System;
using System.Net;
using System.Threading.Tasks;

namespace tests.Mocks
{
    public class MockApi
    {
        private string _prefix;
        private string _resource;

        public string Data { get; set; } = "success";

        private Action<bool, string> completedAction;

        public MockApi(string prefix, string resource, Action<bool, string> callback)
        {
            _prefix = prefix;
            _resource = resource;
            completedAction = callback;
        }

        public void StartApi()
        {
            Console.WriteLine("Test");
            if(!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            var listener = new HttpListener();
            listener.Prefixes.Add(_prefix);
            listener.Start();

            Task.Run(() =>
            {
                var context = listener.GetContext();
                var request = context.Request;

                if (request.HttpMethod != "GET")
                {
                    completedAction(false, "Invalid method");
                    return;
                }

                if (request.Url.AbsoluteUri != (_prefix + _resource))
                {
                    completedAction(false, "Invalid resource");
                    return;
                }

                var response = context.Response;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Data);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();
                listener.Stop();
            });


        }

    }
}