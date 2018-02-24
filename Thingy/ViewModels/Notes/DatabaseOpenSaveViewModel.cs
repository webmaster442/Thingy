using AppLib.Common.Extensions;
using AppLib.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels.Notes
{
    public class DatabaseOpenSaveViewModel : ViewModel
    {
        private IDataBase _db;
        private IApplication _app;

        public ObservableCollection<Note> Notes { get; private set; }

        public DelegateCommand NewNoteCommand { get; }
        public DelegateCommand<Note> DeleteCommand { get; }
        public DelegateCommand<Note> ExportCommand { get; }
        public DelegateCommand ImportCommand { get; }


        public DatabaseOpenSaveViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            Notes = new ObservableCollection<Note>(_db.Notes.GetNotes());
            NewNoteCommand = Command.ToCommand(NewNote);
            DeleteCommand = Command.ToCommand<Note>(Delete, CanDeleteExport);
            ExportCommand = Command.ToCommand<Note>(Export, CanDeleteExport);
            ImportCommand = Command.ToCommand(Import);
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
                    _db.Notes.SaveNote(n);
                    Notes.UpdateWith(_db.Notes.GetNotes());
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
            _db.Notes.DeleteNote(obj.Name);
            Notes.UpdateWith(_db.Notes.GetNotes());
        }

        private bool CanDeleteExport(Note obj)
        {
            return obj != null;
        }

        private async void NewNote()
        {
            var model = new Note();
            var result = await _app.ShowDialog(new Views.Notes.NewNote(), "New Note", model);
            if (result)
            {
                _db.Notes.SaveNote(model);
                Notes.UpdateWith(_db.Notes.GetNotes());
            }
        }
    }
}
