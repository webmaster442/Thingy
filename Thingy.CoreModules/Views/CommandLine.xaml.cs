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

        public CommandLineViewModel ViewModel
        {
            get { return DataContext as CommandLineViewModel; }
        }

        public void Close()
        {
            ViewModel?.ClosingCommand?.Execute(null);
        }

        public TextBox GetTextBox()
        {
            return Console;
        }
    }
}
