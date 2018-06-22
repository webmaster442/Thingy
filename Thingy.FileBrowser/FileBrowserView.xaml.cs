using System.Diagnostics;
using System.Windows.Controls;

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

        private void NavigationException(object sender, string e)
        {
            Debug.WriteLine(e);
        }
    }
}
