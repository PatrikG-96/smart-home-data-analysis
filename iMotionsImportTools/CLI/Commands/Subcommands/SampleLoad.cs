using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
using Newtonsoft.Json;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SampleLoad : ICommand
    {
        private List<Sample> _samples;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SampleLoad(List<Sample> samples)
        {
            _samples = samples;
            KeyWord = "load";
        }

        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            var path = args[0];
            dynamic sampleJson = null;
            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();

                try
                {
                    sampleJson = JsonConvert.DeserializeObject<dynamic>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("could not read file");
                }
            }

            var supportedTypes = sampleJson?.supported_types;
            Console.WriteLine("Supported types: {0}", supportedTypes);
            foreach (var type in supportedTypes)
            {

                try
                {
                    Console.WriteLine("Type: {0}", type);
                    switch ((string)type)
                    {
                        case "velocity":

                            foreach (var definition in sampleJson?.velocity)
                            {
                                var id = (string)definition?.id;
                                if (!IsIdUnique(id))
                                {
                                    Console.WriteLine($"Id '{id}' isn't unique");
                                    continue;
                                }

                                _samples.Add(new VelocitySample
                                {
                                    Id = id
                                });
                            }

                            break;
                        case "position":

                            foreach (var definition in sampleJson?.position)
                            {
                                var id = (string)definition?.id;
                                if (!IsIdUnique(id))
                                {
                                    Console.WriteLine($"Id '{id}' isn't unique");
                                    continue;
                                }

                                _samples.Add(new PositionSample
                                {
                                    Id = id
                                });
                            }

                            break;
                        default:
                            Console.WriteLine("Invalid sample found");
                            break;
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine("Adding sensor failed with error: " + e);
                }

            }
        }




        private bool IsIdUnique(string id)
        {
            foreach (var sample in _samples)
            {
                if (sample.Id == id)
                {
                    return false;
                }
            }

            return true;
        }
    }
}