using System;
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

        public DelegateCommand<string> NavigateCommand { get; }
        public DelegateCommand<string> RunModuleCommand { get; }
        public DelegateCommand<string> RunProgramCommand { get; }
        public DelegateCommand<string> RunModuleDirectoryCommand { get; }
        public ProviderViewModel ItemProvider { get; }

        public FileBrowserViewModel(IApplication app, IDataBase db, IModuleLoader moduleLoader)
        {
            NavigateCommand = Command.CreateCommand<string>(Navigate);
            RunModuleCommand = Command.CreateCommand<string>(RunModule);
            RunModuleDirectoryCommand = Command.CreateCommand<string>(RunModuleDirectory);
            RunProgramCommand = Command.CreateCommand<string>(RunProgram);
            CurrentFolder = FileListView.HomePath;
            ItemProvider = new ProviderViewModel(app, db, moduleLoader);
            CurrentFolder = @"HOME:\";
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
            CurrentFolder = location;
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
