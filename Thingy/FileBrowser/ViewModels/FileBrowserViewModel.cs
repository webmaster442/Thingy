using System;
using System.Diagnostics;
using AppLib.MVVM;
using Thingy.API;
using Thingy.Db;
using Thingy.FileBrowser.Controls;
using Thingy.Implementation;

namespace Thingy.FileBrowser.ViewModels
{
    internal class FileBrowserViewModel: ViewModel
    {

        private string _currentFolder;
        private bool _hiddenvisible;
        private readonly IApplication _app;

        public DelegateCommand<string> NavigateCommand { get; }
        public DelegateCommand<string> RunModuleCommand { get; }
        public DelegateCommand<string> RunProgramCommand { get; }
        public DelegateCommand<string> RunModuleDirectoryCommand { get; }
        public DelegateCommand<string> FileDoubleClickedCommand { get; }
        public ProviderViewModel ItemProvider { get; }

        public FileBrowserViewModel(IApplication app, IDataBase db, IModuleLoader moduleLoader)
        {
            _app = app;
            NavigateCommand = Command.CreateCommand<string>(Navigate);
            RunModuleCommand = Command.CreateCommand<string>(RunModule);
            RunModuleDirectoryCommand = Command.CreateCommand<string>(RunModuleDirectory);
            RunProgramCommand = Command.CreateCommand<string>(RunProgram);
            FileDoubleClickedCommand = Command.CreateCommand<string>(FileDoubleClicked);
            CurrentFolder = FileListView.HomePath;
            ItemProvider = new ProviderViewModel(app, db, moduleLoader);
            CurrentFolder = @"HOME:\";
        }

        private async void FileDoubleClicked(string obj)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = obj;
                p.Start();
            }
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", "File can't be started", DialogButtons.Ok);
                _app.Log.Exception(ex);
            }
        }

        public async void LogNavigationException(string e)
        {
            await _app.ShowMessageBox("Error", "Can't navigate to the specified folder", DialogButtons.Ok);
            _app.Log.Error("Navigation error: {0}", e);
        }

        public string CurrentFolder
        {
            get { return _currentFolder; }
            set
            {
                SetValue(ref _currentFolder, value);
                if (ItemProvider != null)
                {
                    ItemProvider.CurrentFolder = value;
                }
            }
        }

        public bool ShowHiddenFiles
        {
            get { return _hiddenvisible; }
            set { SetValue(ref _hiddenvisible, value); }
        }

        private void Navigate(string location)
        {
            CurrentFolder = ItemProvider.GetPath(location);
        }

        private void RunProgram(string obj)
        {
            ItemProvider.StartProgram(obj);
        }

        private void RunModule(string obj)
        {
            ItemProvider.StartModule(obj, ItemProvider.SelectedPath);
        }

        private void RunModuleDirectory(string obj)
        {
            ItemProvider.StartModule(obj, ItemProvider.CurrentFolder);
        }
    }
}
