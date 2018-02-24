using AppLib.MVVM;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using Thingy.Db;
using Thingy.Views.Notes;

namespace Thingy.ViewModels.Notes
{
    public class NoteEditorViewModel: ViewModel<INoteEditor>
    {
        private string _fileOpen;
        private IApplication _app;
        private IDataBase _db;

        public DelegateCommand<bool> NewFileCommand { get; }
        public DelegateCommand<bool> OpenFileCommand { get; }
        public DelegateCommand SaveFileCommand { get; }
        public DelegateCommand SaveAsCommand { get; }
        public DelegateCommand PrintCommand { get; }

        public NoteEditorViewModel(INoteEditor view, IApplication app, IDataBase db): base(view)
        {
            NewFileCommand = Command.ToCommand<bool>(NewFile);
            OpenFileCommand = Command.ToCommand<bool>(OpenFile);
            SaveFileCommand = Command.ToCommand(SaveFile);
            SaveAsCommand = Command.ToCommand(SaveAs);
            PrintCommand = Command.ToCommand(Print);
        }

        private async Task SaveModified(bool modified)
        {
            if (modified)
            {
                var result = await _app.ShowMessageBox("Notes", $"Save changes to {_fileOpen ?? "untitled"}?", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {

                }
            }
            _fileOpen = null;
            View.ClearText();
        }

        private async void NewFile(bool modified)
        {
            await SaveModified(modified);
        }

        private async void OpenFile(bool modified)
        {
            await SaveModified(modified);
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Files|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _fileOpen = ofd.FileName;
                View.LoadFile(_fileOpen);
            }
        }

        private void SaveFile()
        {
            if (!string.IsNullOrEmpty(_fileOpen))
                View.SaveFile(_fileOpen);
            else
                SaveAs();
        }

        private void SaveAs()
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "Files|*.*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _fileOpen = sfd.FileName;
                View.SaveFile(_fileOpen);
            }
        }

        private void Print()
        {
            View.Print(_fileOpen ?? "untitled");
        }
    }
}
