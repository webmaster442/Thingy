using AppLib.MVVM;
using System.IO;
using Thingy.API;

namespace Thingy.GitBash.ViewModels
{
    public class GitBashViewModel : ViewModel<IGitBashView>
    {
        private IApplication _app;

        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand ChangeFolderCommand { get; }
        public ComplexCommand ComplexCommand { get; }

        public GitBashViewModel(IApplication app, IGitBashView view) : base(view)
        {
            _app = app;
            ExecuteCommand = Command.ToCommand<string>(Execute);
            ChangeFolderCommand = Command.ToCommand(ChangeFolder);
            ComplexCommand = new ComplexCommand(_app, view);
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
                        View.SendText(cmd);
                    }
                }
                else
                {
                    View.SendText(obj);
                }
            }
        }
    }
}
