using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.Infrastructure;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for RunProgramModule.xaml
    /// </summary>
    public partial class RunProgram : UserControl, IHaveCloseTask
    {
        public RunProgram()
        {
            InitializeComponent();
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
                    p.Start();
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
