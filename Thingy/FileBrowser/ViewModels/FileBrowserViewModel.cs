using AppLib.MVVM;
using Thingy.Db;
using Thingy.FileBrowser.Controls;

namespace Thingy.FileBrowser.ViewModels
{
    internal class FileBrowserViewModel: ViewModel
    {

        private string _currentFolder;
        private bool _hiddenvisible;

        public DelegateCommand<string> NavigateCommand { get; }

        public ToolbarViewModel Toolbar { get; }

        public FileBrowserViewModel(IDataBase db)
        {
            NavigateCommand = Command.CreateCommand<string>(Navigate);
            CurrentFolder = FileListView.HomePath;
            Toolbar = new ToolbarViewModel(db);
        }

        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { SetValue(ref _currentFolder, value); }
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
    }
}
