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
        private void Hybernate_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Hybernate);
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Lock);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Logout);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Restart);
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Hybernate);
        }

        private void Sleep_Click(object sender, RoutedEventArgs e)
        {
            PowerManagement.Set(PowerAction.Sleep);
        }

        public WindowsPower()
        {
            InitializeComponent();
        }
    }
}
