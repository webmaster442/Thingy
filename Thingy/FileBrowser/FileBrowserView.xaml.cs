using System.Windows.Controls;
using Thingy.FileBrowser.ViewModels;

namespace Thingy.FileBrowser
{
    /// <summary>
    /// Interaction logic for FileBrowserView.xaml
    /// </summary>
    public partial class FileBrowserView : UserControl
    {
        public FileBrowserView()
        {
            InitializeComponent();
            DirectoryBreadCumb.OnNavigationException += NavigationException;
            DirectoryList.OnNavigationException += NavigationException;
            DirectoryTree.OnNavigationException += NavigationException;
        }

        private FileBrowserViewModel ViewModel
        {
            get { return DataContext as FileBrowserViewModel;  }
        }

        private void NavigationException(object sender, string e)
        {
            ViewModel?.LogNavigationException(e);
        }

        private void DirectoryList_FileDoubleClick(object sender, string e)
        {
            ViewModel?.FileDoubleClickedCommand.Execute(e);
        }
    }
}
