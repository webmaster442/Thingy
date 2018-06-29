using AppLib.Common.Extensions;
using AppLib.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using Thingy.API;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.CoreModules.ViewModels.Notes
{
    public class DatabaseOpenSaveViewModel : ViewModel
    {
        private IDataBase _db;
        private IApplication _app;

        public ObservableCollection<Note> Notes { get; private set; }

        public Note SeletedNote { get; set; }

        public DelegateCommand NewNoteCommand { get; }
        public DelegateCommand<Note> DeleteCommand { get; }
        public DelegateCommand<Note> ExportCommand { get; }
        public DelegateCommand ImportCommand { get; }


        public DatabaseOpenSaveViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            Notes = new ObservableCollection<Note>(_db.Notes.GetAll());
            NewNoteCommand = Command.CreateCommand(NewNote);
            DeleteCommand = Command.CreateCommand<Note>(Delete, CanDeleteExport);
            ExportCommand = Command.CreateCommand<Note>(Export, CanDeleteExport);
            ImportCommand = Command.CreateCommand(Import);
        }

        private void Import()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Files|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var file = File.OpenText(ofd.FileName))
                {
                    Note n = new Note
                    {
                        Name = Path.GetFileNameWithoutExtension(ofd.FileName),
                        Content = file.ReadToEnd()
                    };
                    _db.Notes.Save(n);
                    Notes.UpdateWith(_db.Notes.GetAll());
                }
            }
        }

        private void Export(Note obj)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Files|*.*";
            saveFileDialog.FileName = obj.Name;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var file = File.CreateText(saveFileDialog.FileName))
                {
                    file.Write(obj.Content);
                }
            }
        }

        private void Delete(Note obj)
        {
            _db.Notes.Delete(obj.Name);
            Notes.UpdateWith(_db.Notes.GetAll());
        }

        private bool CanDeleteExport(Note obj)
        {
            return obj != null;
        }

        private async void NewNote()
        {
            var model = new Note();
            var result = await _app.ShowDialog("New Note", new Views.Notes.NewNote(), DialogButtons.OkCancel, true, model);
            if (result)
            {
                _db.Notes.Save(model);
                Notes.UpdateWith(_db.Notes.GetAll());
            }
        }

        public void SaveToNote(string content)
        {
            if (SeletedNote != null)
            {
                SeletedNote.Content = content;
                _db.Notes.Save(SeletedNote);
            }
        }

        public string OpenNote()
        {
            if (SeletedNote != null)
            {
                return SeletedNote.Content;
            }
            return null;
        }
    }
}
