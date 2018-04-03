using AppLib.MVVM;
using CmdHost;
using System.Windows;
using Thingy.CoreModules;

namespace Thingy.CoreModules.ViewModels
{
    public class CommandLineViewModel : ViewModel<ICommandLineView>
    {
        private readonly TerminalController controller;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand ClosingCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }
        public DelegateCommand SetFolderCommand { get; private set; }

        public CommandLineViewModel(ICommandLineView view, string shell = "cmd.exe") : base(view)
        {
            controller = new TerminalController(view);
            controller.SetShell(shell);
            LoadedCommand = Command.ToCommand(Loaded);
            ClosingCommand = Command.ToCommand(Closing);
            SetFolderCommand = Command.ToCommand(SetFolder);

            ClearCommand = Command.ToCommand(Clear);
            RestartCommand = Command.ToCommand(Restart);
            CopyCommand = Command.ToCommand(Copy);
            PasteCommand = Command.ToCommand(Paste);
        }

        private void SetFolder()
        {
            if (View != null)
            {
                var fb = new System.Windows.Forms.FolderBrowserDialog();
                if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Clipboard.SetText($"pushd \"{fb.SelectedPath}\"");
                    View?.GetTextBox()?.Paste();
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
