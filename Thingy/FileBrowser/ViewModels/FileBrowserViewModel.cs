using AppLib.MVVM;
using Thingy.FileBrowser.Controls;

namespace Thingy.FileBrowser.ViewModels
{
    internal class FileBrowserViewModel: ViewModel
    {
        private string _currentFolder;

        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { SetValue(ref _currentFolder, value); }
        }

        public FileBrowserViewModel()
        {
            CurrentFolder = FileListView.HomePath;
        }
    }
}
