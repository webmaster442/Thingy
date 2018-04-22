using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;
using Thingy.API.Capabilities;

namespace Thingy.Mpv.Views
{
    /// <summary>
    /// Interaction logic for YoutubeDlView.xaml
    /// </summary>
    public partial class YoutubeDlView : UserControl, IHaveCloseTask
    {
        private IApplication _app;

        public YoutubeDlView(IApplication app)
        {
            _app = app;
            InitializeComponent();
        }

        private void RunYoutubeDl(string dir, string args)
        {
            Process p = new Process();
            p.StartInfo.FileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\youtube-dl.exe");
            if (string.IsNullOrEmpty(dir))
                p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            else
                p.StartInfo.WorkingDirectory = dir;

            p.StartInfo.Arguments = args;

            p.Start();
        }

        private async Task<string> RunCommand(string dir, string args)
        {
            Process p = new Process();
            p.StartInfo.FileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\youtube-dl.exe");
            if (string.IsNullOrEmpty(dir))
                p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            else
                p.StartInfo.WorkingDirectory = dir;

            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;

            p.Start();

            string result = await p.StandardOutput.ReadToEndAsync();
            await Task.Run(() => p.WaitForExit());

            return result;
        }

        private async void BtnFormats_Click(object sender, RoutedEventArgs e)
        {
            var o = await RunCommand(null, $"-F {TbUrl.Text}");
            await _app.ShowMessageBox("Output", o, DialogButtons.Ok);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            RunYoutubeDl(null, "-U");
        }

        private void Download()
        {
            if (string.IsNullOrEmpty(TbArguments.Text))
                RunYoutubeDl(TbTargetFolder.Text, $"{TbUrl.Text}");
            else
                RunYoutubeDl(TbTargetFolder.Text, $"{TbArguments.Text} {TbUrl.Text}");
        }

        public Action ClosingTask
        {
            get { return Download; }
        }

        public bool CanExecuteAsync
        {
            get { return false; }
        }
    }
}
