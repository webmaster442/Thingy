using System;

namespace Thingy.Cmd
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CommandHost host = new CommandHost();
            host.Run();
        }
    }
}