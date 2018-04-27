using System.Windows;
using System.Windows.Controls;
using Thingy.InternalCode;

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
            PowerManagement.Set(PowerAction.Lock);
        }

        private void Sleep_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Sleep);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Logout);
        }

        private void Hybernate_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Hybernate);
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Hybernate);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Restart);
        }
    }
}
