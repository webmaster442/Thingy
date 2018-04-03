using AppLib.Common.Log;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.Infrastructure;

namespace Thingy.CoreModules.Views
{
    /// <summary>
    /// Interaction logic for RunProgramModule.xaml
    /// </summary>
    public partial class RunProgram : UserControl, IHaveCloseTask
    {
        private ILogger _log;

        public RunProgram(ILogger log)
        {
            InitializeComponent();
            _log = log;
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
                        p.Start();
                    }
                    catch (Win32Exception ex)
                    {
                        _log.Error(ex);
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
