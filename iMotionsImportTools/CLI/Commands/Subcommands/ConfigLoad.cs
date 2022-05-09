using System;
using System.IO;
using iMotionsImportTools.Controller;
using Newtonsoft.Json;

namespace iMotionsImportTools.CLI.Commands.Subcommands
{
    public class ConfigLoad : ICommand
    {
        private OutputLoad _output;
        private SensorLoad _sensorLoad;
        private SampleLoad _sampleLoad;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public ConfigLoad(OutputLoad output, SensorLoad sensorLoad, SampleLoad sampleLoad)
        {
            _output = output;
            _sensorLoad = sensorLoad;
            _sampleLoad = sampleLoad;
            KeyWord = "load";
        }
        public void ExecuteCommand(SensorController controller, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            var path = args[0];
            dynamic configJson = null;
            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();

                try
                {
                    configJson = JsonConvert.DeserializeObject<dynamic>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("could not read file");
                }
            }

            var sensorPath = (string) configJson?.sensor_json_path;
            var outputPath = (string) configJson?.output_json_path;
            var samplePath = (string) configJson?.sample_json_path;

            _output.ExecuteCommand(controller, new []{outputPath});
            _sensorLoad.ExecuteCommand(controller, new []{sensorPath});
            _sampleLoad.ExecuteCommand(controller, new []{samplePath});

        }
    }
}