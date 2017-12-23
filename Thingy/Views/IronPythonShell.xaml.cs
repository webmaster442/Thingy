using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for IronPythonShell.xaml
    /// </summary>
    public partial class IronPythonShell : UserControl
    {
        public IronPythonShell()
        {
            InitializeComponent();
            PythonConsole.Pad.Control.TextArea.FontFamily = FindResource("UbuntuMono") as FontFamily;

        }

        private void FontSizeSelector_ValueChanged(object sender, RoutedEventArgs e)
        {
            PythonConsole.Pad.Control.TextArea.FontSize = FontSizeSelector.Value * 1.4;
        }
    }
}
