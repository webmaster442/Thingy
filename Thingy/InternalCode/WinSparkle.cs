using System.Runtime.InteropServices;

namespace Thingy.InternalCode
{
    internal class WinSparkle
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);

        // Note that some of these functions are not implemented by WinSparkle YET.
        [DllImport("WinSparkle.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_sparkle_init();
        [DllImport("WinSparkle.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_sparkle_cleanup();
        [DllImport("WinSparkle.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_sparkle_set_appcast_url(string url);
        [DllImport("WinSparkle.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_sparkle_set_app_details(string company_name, string app_name, string app_version);
        [DllImport("WinSparkle.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_sparkle_set_registry_path(string path);
        [DllImport("WinSparkle.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_sparkle_check_update_with_ui();
    }
}
