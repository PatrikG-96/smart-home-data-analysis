using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Network;
using iMotionsImportTools.Output;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
using Newtonsoft.Json;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{

    public class OutputLoad : ICommand
    {
        private readonly List<IOutputDevice> _outputDevices;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public OutputLoad(List<IOutputDevice> output)
        {
            KeyWord = "load";
            _outputDevices = output;
        }

        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            var path = args[0];
            dynamic outputJson = null;
            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();

                try
                {
                    outputJson = JsonConvert.DeserializeObject<dynamic>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("could not read file");
                }
            }

            var supportedTypes = outputJson?.supported_types;
            Console.WriteLine("Supported types: {0}", supportedTypes);
            foreach (var type in supportedTypes)
            {

                Console.WriteLine("Type: {0}", type);
                string id;
                try
                {
                    switch ((string) type)
                    {
                        case "stdout":
                            id = (string) outputJson?.stdout?.id;
                            if (!IsIdUnique(id))
                            {
                                Console.WriteLine("id not unique");
                                continue;
                            }

                            _outputDevices.Add(new Stdout
                            {
                                Id = id
                            });

                            break;
                        case "file":

                            foreach (var definition in outputJson?.file)
                            {
                                id = definition?.id;
                                if (!IsIdUnique(id))
                                {
                                    Console.WriteLine("id not unique");
                                    continue;
                                }

                                _outputDevices.Add(new FileOutput((string) definition?.filepath)
                                {
                                    Id = id
                                });

                            }

                            break;

                        case "remote_server":

                            foreach (var definition in outputJson?.remote_server)
                            {
                                id = definition?.id;
                                if (!IsIdUnique(id))
                                {
                                    Console.WriteLine("id not unique");
                                    continue;
                                }

                                var client = new AsyncTcpClient(id);

                                var connectionInfo = new ServerInfo((string) definition?.host, (int) definition?.port);

                                client.Connect(connectionInfo, CancellationToken.None).Wait();

                                _outputDevices.Add(client);
                            }

                            break;
                        default:
                            Console.WriteLine("Invalid output type found");
                            break;
                    }
                }
                catch (AggregateException aggregateException)
                {
                    foreach (var aggEx in aggregateException.InnerExceptions)
                    {
                        Console.WriteLine("Exception: " + aggEx.GetType());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
                
            }
        }


        private bool IsIdUnique(string id)
        {
            foreach (var output in _outputDevices)
            {
                if (output.Id == id)
                {
                    return false;
                }
            }

            return true;
        }
    }
}


