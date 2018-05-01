using AppLib.Common.Extensions;
using AppLib.MVVM;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.JobCore;

namespace Thingy.CoreModules.ViewModels
{
    public class VirtualFoldersViewModel : ViewModel, ICanImportExportXMLData
    {
        private IDataBase _db;
        private IApplication _app;
        private bool _changed;
        private string _selectedfolder;

        public ObservableCollection<VirtualFolder> Folders { get; private set; }
        public ObservableCollection<string> CurrentFolder { get; private set; }

        public DelegateCommand<VirtualFolder> LoadFolderCommand { get; private set; }
        public DelegateCommand NewFolderCommand { get; private set; }
        public DelegateCommand<VirtualFolder> DeleteFolderCommand { get; private set; }
        public DelegateCommand<string[]> FilesDroppedCommand { get; private set; }

        public DelegateCommand ClearFolderCommand { get; private set; }
        public DelegateCommand AddFilesCommand { get; private set; }
        public DelegateCommand<IList> DeleteFilesCommand { get; private set; }
        public DelegateCommand SaveFolderCommand { get; private set; }
        public DelegateCommand CopyContentsCommand { get; private set; }
        public DelegateCommand CreateZipCommand { get; private set; }

        public VirtualFoldersViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            Folders = new ObservableCollection<VirtualFolder>();
            CurrentFolder = new ObservableCollection<string>();
            CurrentFolder.CollectionChanged += CurrentFolder_CollectionChanged;
            Folders.UpdateWith(_db.VirtualFolders.GetVirtualFolders());

            NewFolderCommand = Command.ToCommand(NewFolder);
            DeleteFolderCommand = Command.ToCommand<VirtualFolder>(DeleteFolder, CanDeleteFolder);
            ClearFolderCommand = Command.ToCommand(ClearFolder, IsFolderOpened);
            AddFilesCommand = Command.ToCommand(AddFiles, IsFolderOpened);
            DeleteFilesCommand = Command.ToCommand<IList>(DeleteFiles, CanDeleteFiles);
            CopyContentsCommand = Command.ToCommand(CopyContents, CanCopyOrZip);
            CreateZipCommand = Command.ToCommand(CreateZip, CanCopyOrZip);
            LoadFolderCommand = Command.ToCommand<VirtualFolder>(LoadFolder);
            SaveFolderCommand = Command.ToCommand(SaveFolder, CanSaveFolder);
            FilesDroppedCommand = Command.ToCommand<string[]>(FilesDropped);
        }

        private void FilesDropped(string[] obj)
        {
            if (IsFolderOpened())
            {
                CurrentFolder.AddRange(obj);
            }
        }

        public string SelectedFolder
        {
            get { return _selectedfolder; }
            set { SetValue(ref _selectedfolder, value); }
        }

        private void CurrentFolder_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _changed = true;
        }

        private void LoadFolder(VirtualFolder obj)
        {
            if (obj != null)
            {
                if (obj.Name != SelectedFolder && _changed == true)
                {
                    UpdateVirtualFolder();
                }
                CurrentFolder.UpdateWith(obj.Files);
                SelectedFolder = obj.Name;
                _changed = false;
            }
        }

        private void UpdateVirtualFolder()
        {
            var loadedfolder = Folders.Where(f => f.Name == SelectedFolder)
                                      .FirstOrDefault();
            if (loadedfolder != null)
            {
                loadedfolder.Files.Clear();
                loadedfolder.Files.AddRange(CurrentFolder);
                _db.VirtualFolders.SaveVirtualFolder(loadedfolder);
            }
        }

        private async void NewFolder()
        {
            var modell = new VirtualFolder();
            var result = await _app.ShowDialog("New Virtual Folder", new CoreModules.Dialogs.NewVirtualFolder(), DialogButtons.OkCancel, true, modell);
            if (result)
            {
                _db.VirtualFolders.SaveVirtualFolder(modell);
                Folders.UpdateWith(_db.VirtualFolders.GetVirtualFolders());
            }
        }

        private bool CanSaveFolder()
        {
            return _changed && IsFolderOpened();
        }

        private void SaveFolder()
        {
            UpdateVirtualFolder();
            _changed = false;
        }

        private bool IsFolderOpened()
        {
            return !string.IsNullOrEmpty(_selectedfolder);
        }

        private void ClearFolder()
        {
            CurrentFolder.Clear();
        }

        private bool CanDeleteFolder(VirtualFolder obj)
        {
            return obj != null;
        }

        private void DeleteFolder(VirtualFolder obj)
        {
            var q = MessageBox.Show("Delete virtual folder?", "Virtual Folder delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                _db.VirtualFolders.DeleteVirtualFolder(obj.Name);
            }
        }

        private void AddFiles()
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "All files|*.*",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentFolder.AddRange(openFileDialog.FileNames);
            }
        }

        private void DeleteFiles(IList obj)
        {
            List<string> files = new List<string>(obj.Count);
            files.AddRange(obj.Cast<string>());

            foreach (var file in files)
            {
                CurrentFolder.Remove(file);
            }
        }

        private bool CanDeleteFiles(IList obj)
        {
            return (obj != null && obj.Count > 0);
        }

        private bool CanCopyOrZip()
        {
            return CurrentFolder.Count > 0;
        }

        private void CopyContents()
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select target folder"
            };
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var job = new JobCore.Jobs.CopyFilesToDirectoryJob(CurrentFolder, folderBrowserDialog.SelectedPath);
                var dialog = new JobRunner(_app, job);
                dialog.Show();
            }
        }

        private void CreateZip()
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Zip files|*.zip";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var job = new JobCore.Jobs.CreateZipJob(CurrentFolder, saveFileDialog.FileName);
                var dialog = new JobRunner(_app, job);
                dialog.Show();
            }
        }

        public Task Import(Stream xmlData, bool append)
        {
            return Task.Run(() =>
            {
                var import = EntitySerializer.Deserialize<VirtualFolder[]>(xmlData);
                if (append)
                    _db.VirtualFolders.SaveVirtualFolders(import);
                else
                {
                    _db.VirtualFolders.DeleteAll();
                    _db.VirtualFolders.SaveVirtualFolders(import);
                }
                _app.CurrentDispatcher.Invoke(() => Folders.UpdateWith(_db.VirtualFolders.GetVirtualFolders()));
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
