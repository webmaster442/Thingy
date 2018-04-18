using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace Thingy.GitBash.Native
{
    internal class GitHost : HwndHost
    {
        private Process _process;
        private IntPtr _handler;
        private string _gitPath;

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(_gitPath));
            _process = new Process
            {
                StartInfo = new ProcessStartInfo(_gitPath, "--nodaemon /bin/bash -l -i")
            };
            _process.Start();
            _handler = IntPtr.Zero;
            if (_process.WaitForInputIdle())
            {
                _handler = _process.MainWindowHandle;
            }
            SetParent(hwndParent.Handle, _handler);
            Win32.SetFocus(_handler);
            return new HandleRef(this, _handler);
        }

        protected override bool TabIntoCore(TraversalRequest request)
        {
            return Win32.SetFocus(_handler) > 0;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Win32.DestroyWindow(hwnd.Handle);
            if (_process != null)
            {
                _process.Close();
                _process = null;
            }
        }

        private void SetParent(IntPtr parentHwnd, IntPtr childHwnd)
        {
            int windowLong = Win32.GetWindowLong(childHwnd, -16);
            windowLong = (windowLong & -12582913 & -262145);
            windowLong |= 0x40000000;
            Win32.SetWindowLong(childHwnd, -16, windowLong);
            Win32.SetParent(childHwnd, parentHwnd);
            base.InvalidateVisual();
            Win32.SetFocus(childHwnd);
        }

        public GitHost(string path) : base()
        {
            _gitPath = path;
        }

        private string EscapeForSendKey(string s)
        {
            var escaped = new StringBuilder();
            foreach (var chr in s.Trim())
            {
                switch (chr)
                {
                    case '+':
                    case '^':
                    case '~':
                    case '%':
                    case '(':
                    case ')':
                    case '{':
                    case '}':
                    case '[':
                    case ']':
                        escaped.Append("{");
                        escaped.Append(chr);
                        escaped.Append("}");
                        break;
                    default:
                        escaped.Append(chr);
                        break;
                }
            }
            return escaped.ToString();
        }

        public void SendText(string s)
        {
            if (IsAlive)
            {
                Win32.BringWindowToTop(_handler);
                Win32.SetFocus(_handler);
                var lines = s.Split('\n');
                foreach (var line in lines)
                {
                    var cmd = EscapeForSendKey(line);
                    if (!string.IsNullOrEmpty(cmd))
                    {
                        SendKeys.SendWait(cmd);
                        SendKeys.SendWait("{ENTER}");
                    }
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                try
                {
                    return (_process != null) && Process.GetProcesses().Where(p => p.Id == _process.Id).Any();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }

}
