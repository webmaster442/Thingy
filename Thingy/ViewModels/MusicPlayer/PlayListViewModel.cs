using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Thingy.MusicPlayerCore;
using Thingy.MusicPlayerCore.Formats;

namespace Thingy.ViewModels.MusicPlayer
{
    public class PlayListViewModel: ViewModel
    {
        public ObservableCollection<string> Playlist { get; set; }

        public DelegateCommand OpenListCommand  { get; private set; }
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
        }

        private async Task DoOpenList(bool apend)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.PlalistsFilterString;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = Path.GetExtension(ofd.FileName);
                IEnumerable<string> result = null;
                switch (ext)
                {
                    case ".pls":
                        result = await PlaylistLoaders.LoadPls(ofd.FileName);
                        break;
                    case ".m3u":
                        result = await PlaylistLoaders.LoadM3u(ofd.FileName);
                        break;
                    case ".wpl":
                        result = await PlaylistLoaders.LoadWPL(ofd.FileName);
                        break;
                    case ".asx":
                        result = await PlaylistLoaders.LoadASX(ofd.FileName);
                        break;
                }

                if (result != null)
                {
                    if (!apend) Playlist.Clear();
                    Playlist.AddRange(result);
                }

            }
        }

        private async void AppendList()
        {
            await DoOpenList(true);
        }

        private async void OpenList()
        {
            await DoOpenList(false);
        }

        private void AddFiles()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.AllFormatsFilterString;
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
            throw new NotImplementedException();
        }
    }
}
