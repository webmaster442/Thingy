using System;
using AppLib.MVVM;
using Thingy.FileBrowser.Controls;

namespace Thingy.FileBrowser.ViewModels
{
    internal class FileBrowserViewModel: ViewModel
    {
        private string _currentFolder;
        private bool _hiddenvisible;

        public DelegateCommand<string> NavigateCommand { get; }

        public FileBrowserViewModel()
        {
            NavigateCommand = Command.CreateCommand<string>(Navigate);
            CurrentFolder = FileListView.HomePath;
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

        private void Navigate(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
