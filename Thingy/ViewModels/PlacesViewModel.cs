using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        private string _filter;

        public ObservableCollection<SystemFolderLink> SystemPlaces { get; private set; }
        public ObservableCollection<Drive> Drives { get; private set; }
        public ObservableCollection<FolderLink> Folders { get; private set; }

        public DelegateCommand NewFolderLinkCommand { get; private set; }
        public DelegateCommand<string> OpenLocationCommand { get; private set; }
        public DelegateCommand<string> DeleteSelectedLinkCommand { get; private set; }

        public PlacesViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            SystemPlaces = new ObservableCollection<SystemFolderLink>(Providers.ProvideSystemPlaces());
            Drives = new ObservableCollection<Drive>(Providers.ProvideDriveData());
            Folders = new ObservableCollection<FolderLink>();
            NewFolderLinkCommand = Command.ToCommand(NewFolderLink);
            OpenLocationCommand = Command.ToCommand<string>(OpenLocation);
            DeleteSelectedLinkCommand = Command.ToCommand<string>(DeleteSelectedLink);
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                SetValue(ref _filter, value);
                ApplyFiltering();
            }
        }

        private void ApplyFiltering()
        {
            if (string.IsNullOrEmpty(_filter))
                Folders.UpdateWith(_db.GetFavoriteFolders());
            else
            {
                var match = from frolder in _db.GetFavoriteFolders()
                            where 
                            frolder.Name.Contains(_filter, StringComparison.InvariantCultureIgnoreCase)
                            select frolder;
                Folders.UpdateWith(match);
            }
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
            if (_app.ShowDialog(dialog, "New Folder Link", item) == true)
            {
                _db.SaveFavoriteFolder(item);
                ApplyFiltering();
            }
        }

        private void DeleteSelectedLink(string obj)
        {
            var q = MessageBox.Show("Delete link?", "Link delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                _db.DeleteFavoriteFolder(obj);
                ApplyFiltering();
            }
        }
    }
}
