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

        public ObservableCollection<string> Playlist { get; set; }

        public DelegateCommand OpenListCommand { get; private set; }
        public DelegateCommand ApendListCommand { get; private set; }
        public DelegateCommand AddFilesCommand { get; private set; }
        public DelegateCommand AddUrlCommand { get; private set; }
        public DelegateCommand AddFolderCommand { get; private set; }

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
                if (CurrentIndex > -1 && CurrentIndex < Playlist.Count - 1)
                {
                    return Playlist[CurrentIndex];
                }
                else return null;
            }
        }

        public PlayListViewModel(IApplication app)
        {
            _app = app;
            _extensions = new ExtensionProvider();
            Playlist = new ObservableCollection<string>();
            OpenListCommand = Command.ToCommand(OpenList);
            ApendListCommand = Command.ToCommand(AppendList);
            AddFilesCommand = Command.ToCommand(AddFiles);
            AddFolderCommand = Command.ToCommand(AddFolder);
            AddUrlCommand = Command.ToCommand(AddUrl);

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
                if (!apend) Playlist.Clear();
                Playlist.AddRange(result);
            }
        }

        private async void AppendList()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.PlalistsFilterString;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await DoOpenList(ofd.FileName, true);
            }
        }

        private async void OpenList()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.PlalistsFilterString;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await DoOpenList(ofd.FileName, false);
            }
        }

        private void AddFiles()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.AllFormatsFilterString;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Playlist.AddRange(ofd.FileNames);
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
                Playlist.AddRange(Files);
            }
        }

        private void AddUrl()
        {
            throw new NotImplementedException();
        }

        private void ClearList()
        {
            Playlist.Clear();
        }

        private void DeleteSelected(string[] obj)
        {
            foreach (var item in obj)
            {
                Playlist.Remove(item);
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
            var q = from track in Copy(Playlist)
                    orderby track ascending
                    select track;
            Playlist.UpdateWith(q);
        }

        private void SortDescending()
        {
            var q = from track in Copy(Playlist)
                    orderby track descending
                    select track;
            Playlist.UpdateWith(q);
        }

        private void SortSuffle()
        {
            var q = from track in Copy(Playlist)
                    orderby Guid.NewGuid()
                    select track;
            Playlist.UpdateWith(q);
        }

        public bool IsPossibleToChangeTrack(int trackstoChange)
        {
            if (trackstoChange < 0)
            {
                return (CurrentIndex - trackstoChange) >= 0;
            }
            else
            {
                return (CurrentIndex + trackstoChange) <= (Playlist.Count - 1);
            }
        }
    }
}
