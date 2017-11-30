using AppLib.MVVM;
using CmdHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Thingy.Views;

namespace Thingy.ViewModels
{
    public class CommandLineViewModel: ViewModel<ICommandLineView>
    {
        private readonly TerminalController controller;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand ClosingCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }

        public CommandLineViewModel(ICommandLineView view) : base(view)
        {
            controller = new TerminalController(view);
            LoadedCommand = DelegateCommand.ToCommand(Loaded);
            ClosingCommand = DelegateCommand.ToCommand(Closing);

            ClearCommand = DelegateCommand.ToCommand(Clear);
            RestartCommand = DelegateCommand.ToCommand(Restart);
            CopyCommand = DelegateCommand.ToCommand(Copy);
            PasteCommand = DelegateCommand.ToCommand(Paste);
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
