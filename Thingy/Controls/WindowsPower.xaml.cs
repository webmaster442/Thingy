using AppLib.Common.PInvoke;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for WindowsPower.xaml
    /// </summary>
    public partial class WindowsPower : UserControl
    {
        public WindowsPower()
        {
            InitializeComponent();
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            User32.LockWorkStation();
        }

        private void Sleep_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Suspend, true, true);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            User32.ExitWindowsEx(ExitWindows.EWX_LOGOFF, ShutdownReason.FlagPlanned);
        }

        private void Hybernate_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            User32.ExitWindowsEx(ExitWindows.EWX_POWEROFF, ShutdownReason.FlagPlanned);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            User32.ExitWindowsEx(ExitWindows.EWX_REBOOT, ShutdownReason.FlagPlanned);
        }
    }
}
