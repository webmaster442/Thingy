using AppLib.Common.Console;
using AppLib.MVVM;
using System;
using System.Linq;
using Thingy.API;
using Thingy.Implementation;

namespace Thingy.InternalModules
{
    internal class CommandLineViewModel : ViewModel, ICmdHost
    {
        private string _currentDir;

        private readonly IModuleLoader _loader;

        public CommandLineViewModel(IModuleLoader loader)
        {
            _loader = loader;
            CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            RunCommand = Command.ToCommand<string>(Run);
        }

        public DelegateCommand<string> RunCommand { get; }

        public string CurrentDirectory
        {
            get { return _currentDir; }
            set { SetValue(ref _currentDir, value); }
        }

        private async void Run(string commandline)
        {
            try
            {
                var parser = new ParameterParser(commandline, false, true);
                var cmd = parser.Files[0];
                var arguments = new CmdArguments(parser.StandaloneSwitches, parser.Files.Skip(1).ToList(), parser.SwitchesWithValue);

                var torun = _loader.GetCommandLineModuleByName(cmd);
                if (torun == null)
                {
                    throw new Exception("Unknown command: " + cmd);
                }
                var result = await torun.Run(arguments);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
