using System.Windows.Controls;
using Thingy.API;

namespace Thingy.InternalModules
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        public LogViewer(IApplication app): this()
        {
            LogBox.Text = app.Log.ToString();
            LogBox.ScrollToEnd();
        }
    }
}
