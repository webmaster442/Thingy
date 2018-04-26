using System.Diagnostics;

namespace Thingy.InternalCode
{
    internal enum PowerAction
    {
        Lock,
        Restart,
        Shutdown,
        Logout,
        Hybernate,
        Sleep,
    }

    internal static class PowerManagement
    {
        private static void RunDll32(string invoke)
        {
            Process p = new Process();
            p.StartInfo.FileName = "rundll32.exe";
            p.StartInfo.Arguments = invoke;
            p.Start();
        }

        private static void Shutdwon(string invoke)
        {
            Process p = new Process();
            p.StartInfo.FileName = "shutdown.exe";
            p.StartInfo.Arguments = invoke;
            p.Start();
        }

        public static void Set(PowerAction action)
        {
            switch (action)
            {
                case PowerAction.Lock:
                    RunDll32("user32.dll, LockWorkStation");
                    break;
                case PowerAction.Restart:
                    Shutdwon("/r");
                    break;
                case PowerAction.Logout:
                    Shutdwon("/l");
                    break;
                case PowerAction.Shutdown:
                    Shutdwon("/s");
                    break;
                case PowerAction.Hybernate:
                    Shutdwon("/h");
                    break;
                case PowerAction.Sleep:
                    Shutdwon("powrprof.dll, SetSuspendState 0, 1, 0");
                    break;
            }
        }
    }
}
