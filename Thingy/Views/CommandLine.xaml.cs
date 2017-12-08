using System.Windows.Controls;
using Thingy.ViewModels;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for CommandLine.xaml
    /// </summary>
    public partial class CommandLine : UserControl, ICommandLineView
    {
        public CommandLine()
        {
            InitializeComponent();
        }

        public void Close()
        {
            (DataContext as CommandLineViewModel)?.ClosingCommand?.Execute(null);
        }

        public TextBox GetTextBox()
        {
            return Console;
        }
    }
}
