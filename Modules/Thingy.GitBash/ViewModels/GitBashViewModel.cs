using AppLib.MVVM;
using System;
using System.IO;
using System.Windows.Threading;
using Thingy.API;

namespace Thingy.GitBash.ViewModels
{
    public class GitBashViewModel : ViewModel<IGitBashView>
    {
        private IApplication _app;
        
        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand ChangeFolderCommand { get; }
        public ComplexCommand ComplexCommand { get; }

        private DispatcherTimer _aliveTimer;

        public GitBashViewModel(IApplication app, IGitBashView view) : base(view)
        {
            _app = app;
            ExecuteCommand = Command.ToCommand<string>(Execute);
            ChangeFolderCommand = Command.ToCommand(ChangeFolder);
            ComplexCommand = new ComplexCommand(_app, view);
            _aliveTimer = new DispatcherTimer();
            _aliveTimer.Interval = TimeSpan.FromMilliseconds(150);
            _aliveTimer.Tick += _aliveTimer_Tick;
            _aliveTimer.IsEnabled = true;
        }

        private void _aliveTimer_Tick(object sender, EventArgs e)
        {
            if (!View.IsAlive)
            {
                _aliveTimer.Stop();
                View.Close();
                _app.TabManager.CloseCurrentTab();
            }
        }

        private void ChangeFolder()
        {
            var ofd = new System.Windows.Forms.FolderBrowserDialog();
            if (Directory.Exists(_app.Settings.Get("lastGitFolder", "")))
            {
                ofd.SelectedPath = _app.Settings.Get("lastGitFolder", "");
            }
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _app.Settings.Set("lastGitFolder", ofd.SelectedPath);
                var path = ofd.SelectedPath.Replace(@":", "").Replace(@"\", "/");
                View.SendText($"cd /{path}");
            }
        }

        private async void Execute(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                if (obj.StartsWith("[confirm]"))
                {
                    var cmd = obj.Replace("[confirm]", "");
                    var result = await _app.ShowMessageBox("Confirmation", 
                                                           $"Execute command {cmd}?\r\nWarning: this can't be undone.",
                                                           DialogButtons.YesNo);
                    if (result)
                    {
                        _app.Log.Info("Sending git command: {0}", cmd);
                        View.SendText(cmd);
                    }
                }
                else
                {
                    _app.Log.Info("Sending git command: {0}", obj);
                    View.SendText(obj);
                }
            }
        }
    }
}
