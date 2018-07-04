using AppLib.MVVM.MessageHandler;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.API.Messages;

namespace Thingy.Mpv.Views
{
    /// <summary>
    /// Interaction logic for MpvView.xaml
    /// </summary>
    public partial class MpvView : UserControl, IHaveCloseTask, IMessageClient<HandleFileMessage>
    {
        private IApplication _app;

        public MpvView(IApplication app)
        {
            InitializeComponent();
            Messager.Instance.SubScribe(this);
            _app = app;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Files|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TbFileName.Text = dialog.FileName;
            }
        }

        private void CloseJob()
        {
            if (string.IsNullOrEmpty(TbFileName.Text)) return;

            Process p = new Process();
            p.StartInfo.WorkingDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64");
            p.StartInfo.FileName = "mpv.exe";
            p.StartInfo.Arguments = GetStartParameters();

            try
            {
                _app.Log.Info("Starting mpv with parameters: {0}", p.StartInfo.Arguments);
                p.Start();
            }
            catch (Win32Exception ex)
            {
                _app.Log.Error(ex);
            }
        }

        private string GetStartParameters()
        {
            if (string.IsNullOrEmpty(TbArguments.Text))
                return $"\"{TbFileName.Text}\"";
            else
                return $"{TbArguments} \"{TbFileName.Text}\"";
        }

        public void HandleMessage(HandleFileMessage message)
        {
            if (message.Files.Count > 0)
            {
                var file = message.Files[0];
                TbFileName.Text = file;
            }
        }

        public Action ClosingTask
        {
            get { return CloseJob; }
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
