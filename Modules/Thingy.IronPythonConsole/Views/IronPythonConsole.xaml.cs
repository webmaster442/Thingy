using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;

namespace Thingy.IronPythonConsole.Views
{
    /// <summary>
    /// Interaction logic for IronPythonConsole.xaml
    /// </summary>
    public partial class IronPythonConsole : UserControl
    {
        private readonly IApplication _app;
        private bool _loaded;

        public IronPythonConsole()
        {
            InitializeComponent();
            PythonConsole.Pad.Host.ConsoleCreated += Host_ConsoleCreated;
        }

        private void Host_ConsoleCreated(object sender, EventArgs e)
        {
            PythonConsole.Pad.Console.ConsoleInitialized += Console_ConsoleInitialized;
        }

        private void ToggleRunState(bool runing)
        {
            MenuStop.IsEnabled = !runing;
        }

        private void ConsoleOnScriptFinished(object sender, EventArgs e)
        {
            if (!_loaded) return;
            Dispatcher.Invoke(() => ToggleRunState(false));
        }

        private void ConsoleOnScriptStarting(object sender, EventArgs e)
        {
            if (!_loaded) return;
            Dispatcher.Invoke(() => ToggleRunState(true));
        }

        public IronPythonConsole(IApplication app): this()
        {
            _app = app;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            PythonConsole.Pad.Control.ShowLineNumbers = (bool)CheckBox.IsChecked;
        }

        private void EditableSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            PythonConsole.Pad.Control.FontSize = EditableSlider.Value;
        }

        private void MenuStop_Click(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            Task.Factory.StartNew(() => PythonConsole.Pad.Console.AbortRunningScript());
        }

        private void MenuClear_Click(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            PythonConsole.Pad.Control.Clear();
            PythonConsole.Console.Write(">>>", Microsoft.Scripting.Hosting.Shell.Style.Prompt);
        }

        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            using (var ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Python files | *.py";
                ofd.Multiselect = false;
                ofd.Title = "Select Python File to run";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var fs = System.IO.File.OpenText(ofd.FileName))
                    {
                        PythonConsole.Console.Write(fs.ReadToEnd(), Microsoft.Scripting.Hosting.Shell.Style.Out);
                    }
                }
            }
        }

        private void Console_ConsoleInitialized(object sender, EventArgs e)
        {
            _loaded = true;
            PythonConsole.Pad.Console.ScriptStarting += ConsoleOnScriptStarting;
            PythonConsole.Pad.Console.ScriptFinished += ConsoleOnScriptFinished;
        }
    }
}
