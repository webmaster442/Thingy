using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Thingy.MediaLibary.Models;

namespace Thingy.MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for MediaLibary.xaml
    /// </summary>
    public partial class MediaLibary : UserControl
    {
        public MediaLibary()
        {
            InitializeComponent();
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = DataTree.SelectedItem as string;
            var tag = (e.OriginalSource as TextBlock)?.Tag;
            if (selected != null && tag != null)
            {
                SendToPlayerMenuItem.CommandParameter = DataGrid.SelectedItems;
                DataGrid.Visibility = Visibility.Visible;
                RadioList.Visibility = Visibility.Collapsed;
                (DataContext as ViewModels.MediaLibaryViewModel)?.CategoryQueryCommand.Execute(new string[] { selected, tag.ToString() });
            }
            else if (DataTree.SelectedItem is NavigationItem item && item.Name == "Radio")
            {
                SendToPlayerMenuItem.CommandParameter = RadioList.SelectedItems;
                DataGrid.Visibility = Visibility.Collapsed;
                RadioList.Visibility = Visibility.Visible;
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = DataTree.SelectedItem as string;
            var menuitem = (e.OriginalSource as MenuItem);

            var tag = ((menuitem.Parent as ContextMenu).PlacementTarget as TextBlock).Tag;

            if (selected != null && tag != null)
            {
                (DataContext as ViewModels.MediaLibaryViewModel)?.DeleteQueryCommand.Execute(new string[] { selected, tag.ToString() });
            }
        }
    }
}
