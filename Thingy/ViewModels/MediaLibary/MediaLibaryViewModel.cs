using AppLib.MVVM;
using AppLib.WPF;
using System.Collections.ObjectModel;
using System.Linq;
using Thingy.Db;
using Thingy.Resources;

namespace Thingy.ViewModels.MediaLibary
{
    public class MediaLibaryViewModel: ViewModel
    {
        private IApplication _app;
        private IDataBase _db;

        public ObservableCollection<NavigationItem> Tree { get; }

        public MediaLibaryViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            Tree = new ObservableCollection<NavigationItem>();
            BuildTree();
        }

        private void BuildTree()
        {
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
