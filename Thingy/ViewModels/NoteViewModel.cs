using AppLib.Common.Extensions;
using AppLib.MVVM;
using CommonMark;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels
{
    public class NoteViewModel : ViewModel
    {
        private IApplication _app;
        private IDataBase _db;
        private bool _changed;
        private bool _canedit;

        private string _MarkDownText;
        private string _RenderedText;
        private string _Template;
        private string _SelectedNote;

        public DelegateCommand<Note> LoadNoteCommand { get; private set; }
        public DelegateCommand SaveNoteCommand { get; private set; }
        public DelegateCommand NewNoteCommand { get; private set; }
        public DelegateCommand DeleteNoteCommand { get; private set; }
        public DelegateCommand SaveToFileCommand { get; private set; }
        public DelegateCommand ImportFileCommand { get; private set; }

        public ObservableCollection<Note> Notes { get; private set; }

        public NoteViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            LoadNoteCommand = Command.ToCommand<Note>(LoadNote, HasSelectedNote);
            SaveNoteCommand = Command.ToCommand(SaveNote, CanSave);
            NewNoteCommand = Command.ToCommand(NewNote);
            DeleteNoteCommand = Command.ToCommand(DeleteNote, CanDeleteorSaveFile);
            ImportFileCommand = Command.ToCommand(ImportNote);
            SaveToFileCommand = Command.ToCommand(SaveToFile, CanDeleteorSaveFile);

            _Template = Resources.ResourceLocator.GetResourceFile("html.MarkdownTemplate.html");

            Notes = new ObservableCollection<Note>(_db.Notes.GetNotes());
        }

        private bool HasSelectedNote(Note obj)
        {
            return obj != null;
        }

        public string SelectedNote
        {
            get { return _SelectedNote; }
            set { SetValue(ref _SelectedNote, value); }
        }

        public bool CanEdit
        {
            get { return _canedit; }
            set { SetValue(ref _canedit, value); }
        }

        private void SaveToFile()
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Markdown|*.md|Text|*.txt";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var file = File.CreateText(saveFileDialog.FileName))
                {
                    file.Write(MarkDownText);
                }
            }
        }

        private void ImportNote()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Markdown|*.md|Text|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var file = File.OpenText(ofd.FileName))
                {
                     MarkDownText += file.ReadToEnd();
                }
            }
        }

        private void DeleteNote()
        {
            var q = MessageBox.Show("Delete note?", "Note delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                _db.Notes.DeleteNote(SelectedNote);
                Notes.UpdateWith(_db.Notes.GetNotes());
            }
        }

        private async void NewNote()
        {
            var model = new Note();
            var result = await _app.ShowDialog(new Views.Dialogs.NewNote(), "New Note", model);
            if (result)
            {
                _db.Notes.SaveNote(model);
                Notes.UpdateWith(_db.Notes.GetNotes());
            }
        }

        private bool CanSave()
        {
            return _changed == true && !string.IsNullOrEmpty(SelectedNote);
        }

        private void SaveNote()
        {
            UpdateNote();
            _changed = false;
        }

        private bool CanDeleteorSaveFile()
        {
            CanEdit = !string.IsNullOrEmpty(SelectedNote);
            return CanEdit;
        }

        private void UpdateNote()
        {
            var loadednote = Notes.Where(n => n.Name == SelectedNote)
                                      .FirstOrDefault();
            if (loadednote != null)
            {
                loadednote.Content = string.Copy(MarkDownText);
                _db.Notes.SaveNote(loadednote);
            }
        }

        private void LoadNote(Note obj)
        {
            if (obj != null)
            {
                if (obj.Name != SelectedNote && _changed == true)
                {
                    UpdateNote();
                }
                MarkDownText = obj.Content;
                SelectedNote = obj.Name;
                _changed = false;
            }
        }

        public string Combine(string str)
        {
            StringBuilder output = new StringBuilder();
            output.Append(_Template);
            output.Append(str);
            output.Append("</body></html>");
            return output.ToString();
        }

        public string MarkDownText
        {
            get { return _MarkDownText; }
            set
            {
                if (SetValue(ref _MarkDownText, value))
                {
                    _changed = true;
                    try
                    {
                        RenderedText = Combine(CommonMarkConverter.Convert(_MarkDownText));
                    }
                    catch (CommonMarkException ex)
                    {
                        RenderedText = Combine($"<pre>Render error:\r\n{ex.Message}</pre>");
                    }
                }
            }
        }

        public string RenderedText
        {
            get { return _RenderedText; }
            set { SetValue(ref _RenderedText, value); }
        }
    }
}
