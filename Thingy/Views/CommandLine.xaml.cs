using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            (DataContext as CommandLineViewModel)?.ClosingCommand?.Execute();
        }

        public TextBox GetTextBox()
        {
            return Console;
        }
    }
}
