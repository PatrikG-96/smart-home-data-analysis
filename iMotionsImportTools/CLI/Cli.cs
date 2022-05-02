using System;
using System.Collections.Generic;
using System.Linq;
using iMotionsImportTools.CLI.Commands;
using iMotionsImportTools.Controller;
using iMotionsImportTools.iMotionsProtocol;
using iMotionsImportTools.Output;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;

namespace iMotionsImportTools.CLI
{
    public class Cli
    {

        private IMotionsController _controller;
        private List<ISensor> _sensors;
        private List<IOutputDevice> _outputDevices;
        private List<Sample> _samples;
        private Interpreter _interpreter;

        public Cli(IMotionsController controller)
        {
            _controller = controller;
            _sensors = new List<ISensor>();
            _outputDevices = new List<IOutputDevice>();
            _samples = new List<Sample>();
            _interpreter = new Interpreter();

            _interpreter.AddCommand(new SensorCmd(_sensors));
            _interpreter.AddCommand(new ControllerCmd());
            _interpreter.AddCommand(new OutputCmd(_outputDevices));
            _interpreter.AddCommand(new SampleCmd(_samples));

        }

        public void Start()
        {

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();
                if (input == null) continue;
                string[] splitBySpace = input.Split(' ');
                _interpreter.Interpret(splitBySpace[0], splitBySpace.Skip(1).ToArray(), _controller);

                foreach (var sensor in _sensors)
                {
                    if (sensor is WideFind wide)
                    {
                        Console.WriteLine($"WideFind, Id={wide.Id}, Tag={wide.Tag}");
                    }

                    if (sensor is FibaroSensor fib)
                    {
                        Console.WriteLine($"Fibaro, Id={fib.Id}");
                    }
                }
            }

        }

    }
}