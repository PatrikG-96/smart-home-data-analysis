using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using iMotionsImportTools.CLI.Commands.Subcommands;
using iMotionsImportTools.Controller;
using iMotionsImportTools.Sensor;
using iMotionsImportTools.Sensor.WideFind;
using SensorStatus = iMotionsImportTools.CLI.Commands.Subcommands.SensorStatus;

namespace iMotionsImportTools.CLI.Commands
{
    public class SensorCmd : ICommand
    {

        private List<ICommand> subCommands;

        public string KeyWord { get; set; }
        public OutputBuilder Builder { get; private set; }

        public SensorCmd(List<ISensor> sensors)
        {
            KeyWord = "sensor";
            var create = new SensorCreate(sensors);
            create.AddSensorType("widefind", (s1) =>
            {
                if (s1.Length != 2)
                {
                    Console.WriteLine("Invalid arguments");
                    return null;
                }

                return new WideFind(s1[0], s1[1]);
            });
            create.AddSensorType("fibaro", (s1) =>
            {
                
                if (s1.Length != 4)
                {
                    Console.WriteLine("Invalid arguments");
                    return null;
                }
                
                return new FibaroSensor(s1[0], s1[1], s1[2], s1[3]);
            });
            subCommands = new List<ICommand> {new SensorStatus(), create, new SensorDelete(sensors), new SensorAdd(sensors), new SensorRemove(sensors), new SensorSetAttribute(sensors), 
                                              new SensorLoad(sensors), new SensorAvailable(sensors) };
        }
        public void ExecuteCommand(IMotionsController controller, string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid command");
                return;
            }
            var cmd = FindSubCommand(args[0]);

            if (cmd == null)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            cmd.ExecuteCommand(controller, args.Skip(1).ToArray());
     

        }

        private ICommand FindSubCommand(string keyword)
        {

            foreach (var cmd in subCommands)
            {
                if (cmd.KeyWord == keyword)
                {
                    return cmd;
                }
            }

            return null;
        }
    }
}