using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;
using Thingy.API.Capabilities;

namespace Thingy.CoreModules.Views
{
    /// <summary>
    /// Interaction logic for RunProgramModule.xaml
    /// </summary>
    public partial class RunProgram : UserControl, IHaveCloseTask
    {
        private IApplication _app;

        public RunProgram(IApplication app)
        {
            InitializeComponent();
            _app = app;
        }

        public Task ClosingTask()
        {
            return Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (string.IsNullOrEmpty(TbProgramName.Text)) return;

                    Process p = new Process();
                    p.StartInfo.FileName = TbProgramName.Text;
                    p.StartInfo.Arguments = TbArguments.Text;

                    if (CbAdministrator.IsChecked == true)
                    {
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                    }

                    try
                    {
                        _app.Log.Info("Starting program: {0} Arguments: {1}", p.StartInfo.FileName, p.StartInfo.Arguments);
                        p.Start();
                    }
                    catch (Win32Exception ex)
                    {
                        _app.Log.Error(ex);
                    }
                });
            });
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Programs|*.exe;*.bat;*.cmd;*.ps1";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TbProgramName.Text = $"\"{dialog.FileName}\"";
            }
        }
    }
}
