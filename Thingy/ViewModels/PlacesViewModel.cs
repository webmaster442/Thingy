using AppLib.Common.Extensions;
using AppLib.WPF.MVVM;
using System.Collections.ObjectModel;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Implementation;
using Thingy.Implementation.Models;
using Thingy.Views;

namespace Thingy.ViewModels
{
    public class PlacesViewModel : ViewModel
    {

        private IDataBase _db;
        private IApplication _app;

        public ObservableCollection<SystemFolderLink> SystemPlaces { get; private set; }
        public ObservableCollection<Drive> Drives { get; private set; }
        public ObservableCollection<FolderLink> Folders { get; private set; }

        public DelegateCommand NewFolderLinkCommand { get; private set; }
        public DelegateCommand<string> OpenLocationCommand { get; private set; }

        public PlacesViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            SystemPlaces = new ObservableCollection<SystemFolderLink>(Providers.ProvideSystemPlaces());
            Drives = new ObservableCollection<Drive>(Providers.ProvideDriveData());
            Folders = new ObservableCollection<FolderLink>(_db.GetFavoriteFolders());
            NewFolderLinkCommand = DelegateCommand.ToCommand(NewFolderLink);
            OpenLocationCommand = DelegateCommand<string>.ToCommand(OpenLocation);
        }

        private void OpenLocation(string obj)
        {
            if (string.IsNullOrEmpty(obj)) return;

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = obj;
            p.Start();
        }

        private void NewFolderLink()
        {
            var dialog = new NewFolderLink();
            var item = new FolderLink();
            dialog.DataContext = item;
            if (_app.ShowDialog(dialog, "New Folder Link") == true)
            {
                _db.SaveFavoriteFolder(item);
                Folders.UpdateWith(_db.GetFavoriteFolders());
            }
        }
    }
}
