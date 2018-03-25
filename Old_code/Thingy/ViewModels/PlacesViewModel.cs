﻿using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Implementation;
using Thingy.Implementation.Models;
using Thingy.Infrastructure;

namespace Thingy.ViewModels
{
    public class PlacesViewModel : ViewModel, ICanImportExportXMLData
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
            var dialog = new Views.Dialogs.NewFolderLink();
            var item = new FolderLink();
            var result = await _app.ShowDialog(dialog, "New Folder Link", item);
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
                App.Current.Dispatcher.Invoke(() => Folders.UpdateWith(_db.FavoriteFolders.GetFavoriteFolders()));
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