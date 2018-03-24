using AppLib.WPF.Controls;
using System.Diagnostics;
using System.Windows.Controls;

namespace Thingy.Views.Statusbar
{
    /// <summary>
    /// Interaction logic for MonitorSwitcher.xaml
    /// </summary>
    public partial class MonitorSwitcher : UserControl
    {
        public MonitorSwitcher()
        {
            InitializeComponent();
        }

        private void ScreenClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is ImageButton caller)
            {
                Process p = new Process();
                p.StartInfo.FileName = "DisplaySwitch.exe";
                p.StartInfo.Arguments = caller.Tag.ToString();
                p.Start();
            }
        }
    }
}
