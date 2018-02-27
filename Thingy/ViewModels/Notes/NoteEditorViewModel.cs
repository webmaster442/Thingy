using AppLib.MVVM;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Infrastructure;
using Thingy.Views.Notes;

namespace Thingy.ViewModels.Notes
{
    public class NoteEditorViewModel: ViewModel<INoteEditor>, ICanImportExportXMLData
    {
        private string _fileOpen;
        private IApplication _app;
        private IDataBase _db;

        public DelegateCommand<bool> NewFileCommand { get; }
        public DelegateCommand<bool> OpenFileCommand { get; }
        public DelegateCommand SaveFileCommand { get; }
        public DelegateCommand SaveAsCommand { get; }
        public DelegateCommand PrintCommand { get; }
        public DelegateCommand<bool> OpenNoteDBCommand { get; }
        public DelegateCommand SaveNoteDBCommand { get; }

        public NoteEditorViewModel(INoteEditor view, IApplication app, IDataBase db): base(view)
        {
            _app = app;
            _db = db;
            NewFileCommand = Command.ToCommand<bool>(NewFile);
            OpenFileCommand = Command.ToCommand<bool>(OpenFile);
            SaveFileCommand = Command.ToCommand(SaveFile);
            SaveAsCommand = Command.ToCommand(SaveAs);
            PrintCommand = Command.ToCommand(Print);
            OpenNoteDBCommand = Command.ToCommand<bool>(OpenNoteDB);
            SaveNoteDBCommand = Command.ToCommand(SaveNoteDB);
        }

        private async Task SaveModified(bool modified)
        {
            if (modified)
            {
                var result = await _app.ShowMessageBox("Notes", $"Save changes to {_fileOpen ?? "untitled"}?", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    SaveFile();
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

        public Task Import(Stream xmlData, bool append)
        {
            return Task.Run(() =>
            {
                var import = EntitySerializer.Deserialize<Note[]>(xmlData);
                if (append)
                    _db.Notes.SaveNotes(import);
                else
                {
                    _db.Notes.DeleteAll();
                    _db.Notes.SaveNotes(import);
                }
            });
        }

        public Task Export(Stream xmlData)
        {
            return Task.Run(() =>
            {
                EntitySerializer.Serialize(xmlData, _db.Notes.GetNotes().ToArray());
            });
        }

        private async void OpenNoteDB(bool modified)
        {
            await SaveModified(modified);
            var model = new DatabaseOpenSaveViewModel(_app, _db);
            if (await _app.ShowDialog(new DatabaseOpenSave(), "Open Note from DB", model))
            {
                View.EditorText = model.OpenNote();
            }
        }

        private async void SaveNoteDB()
        {
            var model = new DatabaseOpenSaveViewModel(_app, _db);
            if (await _app.ShowDialog(new DatabaseOpenSave(), "Open Note from DB", model))
            {
                model.SaveToNote(View.EditorText);
            }
        }
    }
}
