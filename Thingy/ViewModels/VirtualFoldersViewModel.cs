using AppLib.Common.Extensions;
using AppLib.MVVM;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels
{
    public class VirtualFoldersViewModel: ViewModel
    {
        private IDataBase _db;
        private IApplication _app;
        private bool _changed;
        private string _selectedfolder;

        public ObservableCollection<VirtualFolder> Folders { get; private set; }
        public ObservableCollection<string> CurrentFolder { get; private set; }

        public DelegateCommand<VirtualFolder> LoadFolderCommand { get; private set; }
        public DelegateCommand NewFolderCommand { get; private set; }
        public DelegateCommand<string> DeleteFolderCommand { get; private set; }
        public DelegateCommand AddFilesCommand { get; private set; }
        public DelegateCommand<string[]> DeleteFilesCommand { get; private set; }
        public DelegateCommand CopyContentsCommand { get; private set; }
        public DelegateCommand CreateZipCommand { get; private set; }

        public VirtualFoldersViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            Folders = new ObservableCollection<VirtualFolder>();
            CurrentFolder = new ObservableCollection<string>();
            CurrentFolder.CollectionChanged += CurrentFolder_CollectionChanged;
            Folders.UpdateWith(_db.GetVirtualFolders());

            NewFolderCommand = DelegateCommand.ToCommand(NewFolder);
            DeleteFolderCommand = DelegateCommand<string>.ToCommand(DeleteFolder, CanDeleteFolder);
            AddFilesCommand = DelegateCommand.ToCommand(AddFiles);
            DeleteFilesCommand = DelegateCommand<string[]>.ToCommand(DeleteFiles, CanDeleteFiles);
            CopyContentsCommand = DelegateCommand.ToCommand(CopyContents, CanCopyAndZip);
            CreateZipCommand = DelegateCommand.ToCommand(CreateZip, CanCopyAndZip);
            LoadFolderCommand = DelegateCommand<VirtualFolder>.ToCommand(LoadFolder);
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
                _db.SaveVirtualFolder(loadedfolder);
            }
        }

        private void NewFolder()
        {
            var modell = new VirtualFolder();
            if (_app.ShowDialog(new Views.NewVirtualFolder(), "New Virtual Folder", modell) == true)
            {
                _db.SaveVirtualFolder(modell);
                Folders.UpdateWith(_db.GetVirtualFolders());
            }
        }

        private bool CanDeleteFolder(string obj)
        {
            return !string.IsNullOrEmpty(obj);
        }

        private void DeleteFolder(string obj)
        {
            _db.DeleteVirtualFolder(obj);
        }

        private void AddFiles()
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "All files|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentFolder.AddRange(openFileDialog.FileNames);
            }
        }

        private void DeleteFiles(string[] obj)
        {
            foreach (var item in obj)
            {
                CurrentFolder.Remove(item);
            }
        }

        private bool CanDeleteFiles(string[] obj)
        {
            return (obj != null && obj.Length > 0);
        }

        private bool CanCopyAndZip()
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

            }
        }

        private void CreateZip()
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Zip files|*.zip";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
    }
}
