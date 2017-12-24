using AppLib.MVVM;
using CommonMark;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels
{
    public class NoteViewModel : ViewModel
    {
        private IApplication _app;
        private IDataBase _db;

        private string _MarkDownText;
        private string _RenderedText;
        private string _Template;
        private string _SelectedNote;

        public DelegateCommand<Note> LoadNoteCommand { get; private set; }
        public DelegateCommand SaveNoteCommand { get; private set; }
        public DelegateCommand NewNoteCommand { get; private set; }
        public DelegateCommand<Note> DeleteNoteCommand { get; private set; }
        public DelegateCommand<Note> SaveToFileCommand { get; private set; }
        public DelegateCommand ImportFileCommand { get; private set; }

        public ObservableCollection<string> Notes { get; private set; }

        public NoteViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            LoadNoteCommand = Command.ToCommand<Note>(LoadNote, HasSelectedNote);
            SaveNoteCommand = Command.ToCommand(SaveNote, CanSave);
            NewNoteCommand = Command.ToCommand(NewNote);
            DeleteNoteCommand = Command.ToCommand<Note>(DeleteNote, HasSelectedNote);
            ImportFileCommand = Command.ToCommand(ImportNote);
            SaveToFileCommand = Command.ToCommand<Note>(SaveToFile, CanSaveFile);

            var executing = Assembly.GetExecutingAssembly();
            using (Stream stream = executing.GetManifestResourceStream("Thingy.Resources.MarkdownTemplate.html"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    _Template = reader.ReadToEnd();
                }
            }
        }

        public string SelectedNote
        {
            get { return _SelectedNote; }
            set { SetValue(ref _SelectedNote, value); }
        }

        private void SaveToFile(Note obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSaveFile(Note obj)
        {
            throw new NotImplementedException();
        }

        private void ImportNote()
        {
            throw new NotImplementedException();
        }

        private void DeleteNote(Note obj)
        {
            throw new NotImplementedException();
        }

        private void NewNote()
        {
            throw new NotImplementedException();
        }

        private bool CanSave()
        {
            throw new NotImplementedException();
        }

        private void SaveNote()
        {
            throw new NotImplementedException();
        }

        private bool HasSelectedNote(Note obj)
        {
            throw new NotImplementedException();
        }

        private void LoadNote(Note obj)
        {
            throw new NotImplementedException();
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
