using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Sensor;

namespace iMotionsImportTools.CLI.Commands
{
    public class SampleCreate : ICommand
    {
        private readonly Dictionary<string, Sample> _sampleTypes;
        private readonly List<Sample> _samples;
        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; }

        public SampleCreate(List<Sample> samples)
        {
            KeyWord = "create";
            _samples = samples;
            _sampleTypes = new Dictionary<string, Sample>();
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {

            if (args.Length >= 2)
            {
                Console.WriteLine("here");
                var type = args[0];


                if (!_sampleTypes.ContainsKey(type))
                {
                    Console.WriteLine("Type doesn't exist");
                    return;
                }

                var sample = _sampleTypes[type];
                var id = args[1];
                sample.Id = id;
                _samples.Add(sample);
            }
        }

        public void AddSampleType(string name, Sample sample)
        {
            _sampleTypes.Add(name, sample);
        }
    }
}