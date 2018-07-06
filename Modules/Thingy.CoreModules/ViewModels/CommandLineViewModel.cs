using AppLib.MVVM;
using CmdHost;
using System.Windows;
using Thingy.CoreModules;

namespace Thingy.CoreModules.ViewModels
{
    public class CommandLineViewModel : ViewModel<ICommandLineView>
    {
        private readonly TerminalController controller;
        private readonly string _initscript;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand ClosingCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }
        public DelegateCommand SetFolderCommand { get; private set; }

        public CommandLineViewModel(ICommandLineView view, string shell = "cmd.exe", string basecommand = null) : base(view)
        {
            controller = new TerminalController(view);
            _initscript = basecommand;
            controller.SetShell(shell);
            LoadedCommand = Command.CreateCommand(Loaded);
            ClosingCommand = Command.CreateCommand(Closing);
            SetFolderCommand = Command.CreateCommand(SetFolder);

            ClearCommand = Command.CreateCommand(Clear);
            RestartCommand = Command.CreateCommand(Restart);
            CopyCommand = Command.CreateCommand(Copy);
            PasteCommand = Command.CreateCommand(Paste);
        }

        public void SetShellFolder(string path)
        {
            controller.InvokeCmd("Setting path...", $"pushd \"{path}\"");
        }

        private void SetFolder()
        {
            if (View != null)
            {
                var fb = new System.Windows.Forms.FolderBrowserDialog();
                if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetShellFolder(fb.SelectedPath);
                }
            }
        }

        private void Closing()
        {
            controller.Close();
        }

        private void Loaded()
        {
            controller.Init();
            if (!string.IsNullOrEmpty(_initscript))
            {
                controller.InvokeCmd("Running init script...\r\n", _initscript);
            }
        }

        private void Clear()
        {
            controller.ClearOutput();
        }

        private void Restart()
        {
            controller.RestartProc();
        }

        private void Copy()
        {
            Clipboard.SetText(View?.GetTextBox()?.SelectedText);
        }

        private void Paste()
        {
            View?.GetTextBox()?.Paste();
        }
    }
}
