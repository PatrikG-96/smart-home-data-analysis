using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class SensorLoad : ICommand
    {
        private readonly List<ISensor> _sensors;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        private readonly OutputBuilder sensorBuilder;

        public SensorLoad(List<ISensor> sensors)
        {
            KeyWord = "load";
            _sensors = sensors;

            Builder = new OutputBuilder();
            sensorBuilder = new OutputBuilder();

            Builder.AddTitle("title");
            Builder.AddAttribute("Command");
            Builder.AddAttribute("Status");
            Builder.AddAttribute("Error");
            Builder.AddAttribute("Sensors");

            sensorBuilder.AddTitle("title");
            sensorBuilder.AddAttribute("ID");
            sensorBuilder.AddAttribute("Host");
            sensorBuilder.AddAttribute("Tag");
            sensorBuilder.AddAttribute("Devices");
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {

            Builder.BindValue("title", "Sensor");
            Builder.BindValue("Command", "available");
            if (args.Length != 1)
            {
                Builder.BindValue("Status", "Failed");
                Builder.BindValue("Error", $"Expected 1 arguments, received {args.Length}.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
                return;
            }

            var path = args[0];
            dynamic sensor_json = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path);
                string json = sr.ReadToEnd();
                sensor_json = JsonConvert.DeserializeObject<dynamic>(json);
            }
            catch (Exception e)
            {
                Builder.BindValue("Status", "Failed");
                Builder.BindValue("Error", $"File '{path}' could not be parsed to JSON.");
                Console.WriteLine(Builder.Build());
                Builder.Reset();
                return;
            }
            finally
            {
                if (sr is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }


            var supportedTypes = sensor_json?.supported_types;
            List<string> errors = new List<string>();
            string sensorOutput = "";
            foreach (var type in supportedTypes)
            {

                try
                {
                    Console.WriteLine("Type: {0}", type);

                    switch ((string) type) 
                    {
                        case "widefind":

                            foreach (var definition in sensor_json?.widefind)
                            {

                                var id = (string)definition?.id;
                                if (!IsIdUnique(id))
                                {
                                    errors.Add($"Id '{id}' isn't unique"); 
                                    continue;
                                }

                                var sensor = new WideFind(id, (string) definition?.host)
                                {
                                    Tag = (string) definition?.tag
                                };
                                _sensors.Add(sensor);

                                OutputBuilder.StandardSensorOutput(sensor, sensorBuilder);
                                sensorOutput += sensorBuilder.Build() + "\n";
                                sensorBuilder.Reset();
                            }

                            break;
                        case "fibaro":
                                
                            foreach (var definition in sensor_json?.fibaro)
                            {
                                var id = (string)definition?.id;
                                if (!IsIdUnique(id))
                                {
                                    errors.Add($"Id '{id}' isn't unique");
                                    continue;
                                }

                                var fib = new FibaroSensor(id, (string) definition?.host,
                                                             (string) definition?.user, (string) definition?.password);

                                foreach (var device in definition?.devices)
                                {
                                    fib.AddDevice((int)device);
                                }
                                _sensors.Add(fib);
                                OutputBuilder.StandardSensorOutput(fib, sensorBuilder);
                                sensorOutput += sensorBuilder.Build() + "\n";
                                sensorBuilder.Reset();
                            }

                            break;
                        default:
                            errors.Add($"Invalid sensor type found: '{type}'");
                            break;
                    }


                }
                catch (Exception e)
                {
                    Builder.BindValue("Status", "Failed");
                    Builder.BindValue("Error", $"Adding sensor failed with error: '{e.Message}'");
                    Console.WriteLine(Builder.Build());
                    Builder.Reset();
                    return;
                }
                
            }
            Builder.BindValue("Status", "Success");
            if (errors.Count > 0) Builder.BindValue("Error", string.Join(", ", errors.ToArray()));
            Console.WriteLine(Builder.Build());
            Builder.Reset();
            Console.WriteLine(sensorOutput.Substring(0, sensorOutput.Length-1));
        }


        

        private bool IsIdUnique(string id)
        {
            foreach (var sensor in _sensors)
            {
                if (sensor.Id == id)
                {
                    return false;
                }
            }

            return true;
        }
    }
}