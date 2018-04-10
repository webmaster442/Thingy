using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thingy.MusicPlayerCore;
using Thingy.MusicPlayerCore.Formats;

namespace Thingy.ViewModels.MusicPlayer
{
    public class PlayListViewModel : ViewModel
    {
        private int _currentIndex;

        public ObservableCollection<string> List { get; set; }

        public DelegateCommand OpenListCommand { get; private set; }
        public DelegateCommand ApendListCommand { get; private set; }
        public DelegateCommand AddFilesCommand { get; private set; }
        public DelegateCommand AddUrlCommand { get; private set; }
        public DelegateCommand AddFolderCommand { get; private set; }
        public DelegateCommand SaveListCommand { get; private set; }
        public DelegateCommand LoadCDCommand { get; private set; }

        public DelegateCommand ClearListCommand { get; private set; }
        public DelegateCommand<string[]> DeleteSelectedCommand { get; private set; }

        public DelegateCommand SortAscendingCommand { get; private set; }
        public DelegateCommand SortDescendingCommand { get; private set; }
        public DelegateCommand SortSuffleCommand { get; private set; }

        private IApplication _app;
        private IExtensionProvider _extensions;

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetValue(ref _currentIndex, value); }
        }

        public string CurrrentFile
        {
            get
            {
                if (CurrentIndex > -1 && CurrentIndex < List.Count - 1)
                {
                    return List[CurrentIndex];
                }
                else return null;
            }
        }

        public PlayListViewModel(IApplication app)
        {
            _app = app;
            _extensions = new ExtensionProvider();
            List = new ObservableCollection<string>();
            OpenListCommand = Command.ToCommand(OpenList);
            ApendListCommand = Command.ToCommand(AppendList);
            AddFilesCommand = Command.ToCommand(AddFiles);
            AddFolderCommand = Command.ToCommand(AddFolder);
            AddUrlCommand = Command.ToCommand(AddUrl);
            SaveListCommand = Command.ToCommand(SaveList);
            LoadCDCommand = Command.ToCommand(LoadCD);

            ClearListCommand = Command.ToCommand(ClearList);
            DeleteSelectedCommand = Command.ToCommand<string[]>(DeleteSelected);

            SortAscendingCommand = Command.ToCommand(SortAscending);
            SortDescendingCommand = Command.ToCommand(SortDescending);
            SortSuffleCommand = Command.ToCommand(SortSuffle);
        }

        public async Task DoOpenList(string file, bool apend)
        {
            string ext = Path.GetExtension(file);
            IEnumerable<string> result = null;
            switch (ext)
            {
                case ".pls":
                    result = await PlaylistLoaders.LoadPls(file);
                    break;
                case ".m3u":
                case ".m3u8":
                    result = await PlaylistLoaders.LoadM3u(file);
                    break;
                case ".wpl":
                    result = await PlaylistLoaders.LoadWPL(file);
                    break;
                case ".asx":
                    result = await PlaylistLoaders.LoadASX(file);
                    break;
            }

            if (result != null)
            {
                if (!apend) List.Clear();
                List.AddRange(result);
            }
        }

        private async void AppendList()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.PlalistsFilterString;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    await DoOpenList(ofd.FileName, true);
                }
                catch (Exception ex)
                {
                    await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void OpenList()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.PlalistsFilterString;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    await DoOpenList(ofd.FileName, false);
                }
                catch (Exception ex)
                {
                    await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void SaveList()
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "M3U list|*.m3u";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var target = File.CreateText(sfd.FileName))
                    {
                        foreach (var file in List)
                        {
                            target.WriteLine(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                     await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                }
            }
        }

        private void AddFiles()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.AllFormatsFilterString;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List.AddRange(ofd.FileNames);
            }
        }

        private void AddFolder()
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "Select folder to add";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> Files = new List<string>(30);
                foreach (var filter in _extensions.AllSupportedFormats)
                {
                    Files.AddRange(Directory.GetFiles(fbd.SelectedPath, filter));
                }
                Files.Sort();
                List.AddRange(Files);
            }
        }

        private async void AddUrl()
        {
            var dialog = new Views.MusicPlayer.AddURLDialog();
            bool result = await _app.ShowDialog(dialog, "Add URL...");
            if (result)
            {
                List.Add(dialog.Url);
            }
        }

        private async void LoadCD()
        {
            var dialog = new Views.MusicPlayer.LoadCdDialog();
            bool result = await _app.ShowDialog(dialog, "Load CD...");
            if (result)
            {
                if (!string.IsNullOrEmpty(dialog.SelectedDrive))
                {
                    var tracks = await CDInfoProvider.GetCdTracks(dialog.SelectedDrive);
                    List.AddRange(tracks);
                }
            }
        }

        private void ClearList()
        {
            List.Clear();
        }

        private void DeleteSelected(string[] obj)
        {
            foreach (var item in obj)
            {
                List.Remove(item);
            }
        }

        private static IEnumerable<string> Copy(IEnumerable<string> source)
        {
            foreach (var item in source)
            {
                yield return string.Copy(item);
            }
        }

        private void SortAscending()
        {
            var q = from track in Copy(List)
                    orderby track ascending
                    select track;
            List.UpdateWith(q);
        }

        private void SortDescending()
        {
            var q = from track in Copy(List)
                    orderby track descending
                    select track;
            List.UpdateWith(q);
        }

        private void SortSuffle()
        {
            var q = from track in Copy(List)
                    orderby Guid.NewGuid()
                    select track;
            List.UpdateWith(q);
        }

        public bool IsPossibleToChangeTrack(int trackstoChange)
        {
            if (trackstoChange < 0)
            {
                return (CurrentIndex - trackstoChange) >= 0;
            }
            else
            {
                return (CurrentIndex + trackstoChange) <= (List.Count - 1);
            }
        }
    }
}
