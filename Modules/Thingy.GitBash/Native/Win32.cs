using System;
using System.Runtime.InteropServices;

namespace Thingy.GitBash.Native
{
    internal static class Win32
    {
        public const int SWP_NOZORDER = 4;
        public const int SWP_NOACTIVATE = 16;
        public const int GWL_STYLE = -16;
        public const int WS_CAPTION = 12582912;
        public const int WS_THICKFRAME = 262144;
        public const int WS_CHILD = 1073741824;
        public const int WS_VISIBLE = 268435456;
        public const int WS_EX_TRANSPARENT = 32;
        public const int WS_EX_LAYERED = 524288;
        public const int LWA_ALPHA = 2;
        public const int LWA_COLORKEY = 1;

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        [DllImport("user32")]
        public static extern int SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);
    }
}
