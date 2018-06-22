using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.CoreModules;
using Thingy.CoreModules.Models;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.CoreModules.ViewModels
{
    public class PlacesViewModel : ViewModel, ICanImportExportXMLData
    {

        private IDataBase _db;
        private IApplication _app;
        private string _filter;

        public ObservableCollection<SystemFolderLink> SystemPlaces { get; private set; }
        public ObservableCollection<SystemFolderLink> EnvVars { get; private set; }
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
            EnvVars = new ObservableCollection<SystemFolderLink>(Providers.ProvideEnvironmentVariables());
            Folders = new ObservableCollection<FolderLink>();
            NewFolderLinkCommand = Command.CreateCommand(NewFolderLink);
            OpenLocationCommand = Command.CreateCommand<string>(OpenLocation);
            DeleteSelectedLinkCommand = Command.CreateCommand<string>(DeleteSelectedLink);
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
                Folders.UpdateWith(_db.FavoriteFolders.GetFavoriteFolders());
            else
            {
                var match = from frolder in _db.FavoriteFolders.GetFavoriteFolders()
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

        private async void NewFolderLink()
        {
            var dialog = new CoreModules.Dialogs.NewFolderLink();
            var item = new FolderLink();
            var result = await _app.ShowDialog("New Folder Link", dialog, DialogButtons.OkCancel, true, item);
            if (result)
            {
                _db.FavoriteFolders.SaveFavoriteFolder(item);
                ApplyFiltering();
            }
        }

        private void DeleteSelectedLink(string obj)
        {
            var q = MessageBox.Show("Delete link?", "Link delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                _db.FavoriteFolders.DeleteFavoriteFolder(obj);
                ApplyFiltering();
            }
        }

        public Task Import(Stream xmlData, bool append)
        {
            return Task.Run(() =>
            {
                var import = EntitySerializer.Deserialize<FolderLink[]>(xmlData);
                if (append)
                    _db.FavoriteFolders.SaveFavoriteFolders(import);
                else
                {
                    _db.FavoriteFolders.DeleteAll();
                    _db.FavoriteFolders.SaveFavoriteFolders(import);
                }
                _app.CurrentDispatcher.Invoke(() => Folders.UpdateWith(_db.FavoriteFolders.GetFavoriteFolders()));
            });
        }

        public Task Export(Stream xmlData)
        {
            return Task.Run(() =>
            {
                EntitySerializer.Serialize(xmlData, Folders.ToArray());
            });
        }
    }
}
