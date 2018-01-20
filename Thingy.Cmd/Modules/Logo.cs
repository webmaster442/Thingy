using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Cmd.Modules
{
    internal static class Logo
    {
        public static void PutLogo()
        {
            Console.WriteLine(@" _____ _     _                   ");
            Console.WriteLine(@"|_   _| |__ (_)_ __   __ _ _   _ ");
            Console.WriteLine(@"  | | | '_ \| | '_ \ / _` | | | |");
            Console.WriteLine(@"  | | | | | | | | | | (_| | |_| |");
            Console.WriteLine(@"  |_| |_| |_|_|_| |_|\__, |\__, |");
            Console.WriteLine(@"                     |___/ |___/ ");
            GetAssemblyVersion();
        }

        private static void GetAssemblyVersion()
        {
            var executing = System.Reflection.Assembly.GetExecutingAssembly();
            Console.WriteLine("Version: {0}", executing.GetName().Version.ToString());
        }
    }
}
