using System;
using System.Linq;

namespace Thingy.Cmd
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var trimmedArgs = args.Skip(1).ToArray();

                switch (args[0])
                {
                    case "ipy":
                        Modules.PythonConsoleHost.PythonShell(trimmedArgs);
                        break;
                    default:
                        Console.WriteLine("Unknown module: {0}", args[0]);
                        break;
                }
            }
            else
            {
                Modules.Logo.PutLogo();
            }
        }
    }
}