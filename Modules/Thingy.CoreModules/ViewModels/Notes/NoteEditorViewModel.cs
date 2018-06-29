using AppLib.MVVM;
using CommonMark;
using ICSharpCode.AvalonEdit;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.CoreModules.Views.Notes;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.CoreModules.ViewModels.Notes
{
    public class NoteEditorViewModel: ViewModel<INoteEditor>, ICanImportExportXMLData
    {
        private string _fileOpen;
        private IApplication _app;
        private IDataBase _db;
        private string _Template;

        public DelegateCommand<bool> NewFileCommand { get; }
        public DelegateCommand<bool> OpenFileCommand { get; }
        public DelegateCommand SaveFileCommand { get; }
        public DelegateCommand SaveAsCommand { get; }
        public DelegateCommand PrintCommand { get; }
        public DelegateCommand<bool> OpenNoteDBCommand { get; }
        public DelegateCommand SaveNoteDBCommand { get; }
        public DelegateCommand<string> RenderCommand { get; }
        public DelegateCommand<TextEditor> FindCommand { get; }
        public DelegateCommand<TextEditor> ReplaceCommand { get; }

        public NoteEditorViewModel(INoteEditor view, IApplication app, IDataBase db): base(view)
        {
            _app = app;
            _db = db;
            NewFileCommand = Command.CreateCommand<bool>(NewFile);
            OpenFileCommand = Command.CreateCommand<bool>(OpenFile);
            SaveFileCommand = Command.CreateCommand(SaveFile);
            SaveAsCommand = Command.CreateCommand(SaveAs);
            PrintCommand = Command.CreateCommand(Print);
            OpenNoteDBCommand = Command.CreateCommand<bool>(OpenNoteDB);
            SaveNoteDBCommand = Command.CreateCommand(SaveNoteDB);
            RenderCommand = Command.CreateCommand<string>(Render);
            FindCommand = Command.CreateCommand<TextEditor>(Find);
            ReplaceCommand = Command.CreateCommand<TextEditor>(Replace);
            
            _Template = Resources.ResourceLocator.GetResourceFile("html.MarkdownTemplate.html");
        }

        private async void Replace(TextEditor obj)
        {
            var dialog = new FindReplaceDialog(obj);
            dialog.ConfigureFor(FindReplaceDialog.DialogType.Replace);
            await _app.ShowDialog("Find & Replace", dialog, DialogButtons.None, false);
        }

        private async void Find(TextEditor obj)
        {
            var dialog = new FindReplaceDialog(obj);
            dialog.ConfigureFor(FindReplaceDialog.DialogType.Find);
            await _app.ShowDialog("Find & Replace", dialog, DialogButtons.None, false);
        }

        private async Task SaveModified(bool modified)
        {
            if (modified)
            {
                var result = await _app.ShowMessageBox("Notes", $"Save changes to {_fileOpen ?? "untitled"}?", DialogButtons.YesNo);
                if (result)
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
                    _db.Notes.Save(import);
                else
                {
                    _db.Notes.DeleteAll();
                    _db.Notes.Save(import);
                }
            });
        }

        public Task Export(Stream xmlData)
        {
            return Task.Run(() =>
            {
                EntitySerializer.Serialize(xmlData, _db.Notes.GetAll().ToArray());
            });
        }

        private async void OpenNoteDB(bool modified)
        {
            await SaveModified(modified);
            var model = new DatabaseOpenSaveViewModel(_app, _db);
            if (await _app.ShowDialog("Open Note from DB", new DatabaseOpenSave(), DialogButtons.OkCancel, true, model))
            {
                View.EditorText = model.OpenNote();
            }
        }

        private async void SaveNoteDB()
        {
            var model = new DatabaseOpenSaveViewModel(_app, _db);
            if (await _app.ShowDialog("Open Note from DB", new DatabaseOpenSave(), DialogButtons.OkCancel, true, model))
            {
                model.SaveToNote(View.EditorText);
            }
        }

        private string Combine(string str)
        {
            var output = new StringBuilder();
            output.Append(_Template);
            output.Append(str);
            output.Append("</body></html>");
            return output.ToString();
        }

        private string RenderMD(string s)
        {
            try
            {
                return Combine(CommonMarkConverter.Convert(s));
            }
            catch (CommonMarkException ex)
            {
                return Combine($"<pre>Render error:\r\n{ex.Message}</pre>");
            }
        }

        private async void Render(string obj)
        {
            string content = "";
            switch (obj.ToLower())
            {
                case "html":
                    content = View.GetHTMLString();
                    break;
                case "md":
                    content = RenderMD(View.EditorText);
                    break;
                default:
                    return;
            }

            var control = new Views.Notes.Preivew();
            control.SetContent(content);
            await _app.ShowDialog("Preview", control, DialogButtons.Ok, false);
        }
    }
}
