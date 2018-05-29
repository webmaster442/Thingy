using System;
using System.Diagnostics;

namespace Thingy.Cmd
{
    internal class CommandHost: ICmdHost
    {
        private ModuleRunner _moduleRunner;

        public CommandHost()
        {
            WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _moduleRunner = new ModuleRunner();
        }

        public void Run()
        {
            string input;
            DoLogo();
            do
            {
                WritePrompt();
                input = Console.ReadLine().ToLower();
                try
                {
                    Parameters arguments = ParameterParser.Parse(input, out string command);
                    IModule module = _moduleRunner.GetModule(command);
                    if (module != null)
                    {
                        module.Run(this, arguments);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
                    Debugger.Break();
#endif
                    Console.WriteLine("Error in module. Module execution terminated");
                }
            }
            while (input != "exit");
        }

        private void DoLogo()
        {
            Console.WriteLine(@" _____ _     _                   ");
            Console.WriteLine(@"|_   _| |__ (_)_ __   __ _ _   _ ");
            Console.WriteLine(@"  | | | '_ \| | '_ \ / _` | | | |");
            Console.WriteLine(@"  | | | | | | | | | | (_| | |_| |");
            Console.WriteLine(@"  |_| |_| |_|_|_| |_|\__, |\__, |");
            Console.WriteLine(@"                     |___/ |___/ ");
            var executing = System.Reflection.Assembly.GetExecutingAssembly();
            Console.WriteLine("Version: {0}", executing.GetName().Version.ToString());
            Console.WriteLine();
        }

        private void WritePrompt()
        {
            Console.WriteLine(WorkingDirectory);
            Console.Write("$ ");
        }

        #region ICmdHost implementation
        public string WorkingDirectory { get; set; }

        public void Clear()
        {
            Console.Clear();
        }

        public void Write(string str)
        {
            Console.Write(str);
        }

        public void Write(string str, params object[] args)
        {
            Console.Write(str, args);
        }

        public void WriteLine(string str)
        {
            Console.Write(str);
        }

        public void WriteLine(string str, params object[] args)
        {
            Console.Write(str, args);
        }
        #endregion
    }
}
