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
using Thingy.API;

namespace Thingy.IronPythonConsole.Views
{
    /// <summary>
    /// Interaction logic for IronPythonConsole.xaml
    /// </summary>
    public partial class IronPythonConsole : UserControl
    {
        private readonly IApplication _app;

        public IronPythonConsole()
        {
            InitializeComponent();
        }

        public IronPythonConsole(IApplication app): this()
        {
            _app = app;
        }
    }
}
