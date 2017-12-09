using AppLib.MVVM;
using System.Windows;
using System.Windows.Controls;
using Thingy.ViewModels;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for VirtualFolders.xaml
    /// </summary>
    public partial class VirtualFolders : UserControl, IView<VirtualFoldersViewModel>
    {
        public VirtualFolders()
        {
            InitializeComponent();
        }

        public VirtualFoldersViewModel ViewModel
        {
            get
            {
                if (DataContext != null)
                    return DataContext as VirtualFoldersViewModel;
                else
                    return null;
            }
        }

        private void FilesList_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ViewModel?.FilesDroppedCommand.Execute(files);
        }

        private void FilesList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;

            else
                e.Effects = DragDropEffects.None;
        }
    }
}
