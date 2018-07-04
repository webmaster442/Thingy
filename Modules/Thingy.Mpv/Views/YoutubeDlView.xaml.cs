using AppLib.MVVM.MessageHandler;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.API.Messages;

namespace Thingy.Mpv.Views
{
    /// <summary>
    /// Interaction logic for YoutubeDlView.xaml
    /// </summary>
    public partial class YoutubeDlView : UserControl, IHaveCloseTask, IMessageClient<HandleFileMessage>
    {
        private IApplication _app;

        public YoutubeDlView(IApplication app)
        {
            InitializeComponent();
            Messager.Instance.SubScribe(this);
            _app = app;
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

        public void HandleMessage(HandleFileMessage message)
        {
            if (message.Files.Count > 0)
            {
                var file = message.Files[0];
                TbUrl.Text = file;
            }
        }

        public Action ClosingTask
        {
            get { return Download; }
        }

        public bool CanExecuteAsync
        {
            get { return false; }
        }

        public Guid MessageReciverID
        {
            get { return Guid.Parse(Tag.ToString()); }
        }
    }
}
