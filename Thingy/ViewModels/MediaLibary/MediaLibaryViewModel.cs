using AppLib.MVVM;
using AppLib.WPF;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Thingy.Db;
using Thingy.MusicPlayerCore;
using Thingy.MusicPlayerCore.Formats;
using Thingy.Resources;

namespace Thingy.ViewModels.MediaLibary
{
    public class MediaLibaryViewModel: ViewModel
    {
        private IApplication _app;
        private IDataBase _db;
        private IExtensionProvider _extensions;

        public ObservableCollection<NavigationItem> Tree { get; }
        public DelegateCommand AddFilesCommand { get; }

        public MediaLibaryViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            _extensions = new ExtensionProvider();
            Tree = new ObservableCollection<NavigationItem>();
            AddFilesCommand = Command.ToCommand(AddFiles);
            BuildTree();
        }

        private async void AddFiles()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = _extensions.AllFormatsFilterString;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var items = await DBFactory.CreateSongs(ofd.FileNames);
                _db.MediaLibary.AddSongs(items);
                BuildTree();
                _db.MediaLibary.SaveCache();
            }
        }

        private void BuildTree()
        {
            Tree.Clear();

            Tree.Add(new NavigationItem
            {
                Name = "Albums",
                Icon = BitmapHelper.FrozenBitmap(ResourceLocator.GetIcon(IconCategories.Small, "icons8-music-record-48.png")),
                SubItems = new ObservableCollection<string>(_db.MediaLibary.GetAlbums())
            });
            Tree.Add(new NavigationItem
            {
                Name = "Artists",
                Icon = BitmapHelper.FrozenBitmap(ResourceLocator.GetIcon(IconCategories.Small, "icons8-dj-48.png")),
                SubItems = new ObservableCollection<string>(_db.MediaLibary.GetArtists())
            });
            Tree.Add(new NavigationItem
            {
                Name = "Years",
                Icon = BitmapHelper.FrozenBitmap(ResourceLocator.GetIcon(IconCategories.Small, "icons8-calendar-7-48.png")),
                SubItems = new ObservableCollection<string>(_db.MediaLibary.GetYears().Cast<string>())
            });
            Tree.Add(new NavigationItem
            {
                Name = "Genres",
                Icon = BitmapHelper.FrozenBitmap(ResourceLocator.GetIcon(IconCategories.Small, "icons8-musical-notes-48.png")),
                SubItems = new ObservableCollection<string>(_db.MediaLibary.GetGeneires())
            });
        }
    }
}
