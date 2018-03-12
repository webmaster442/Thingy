using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Views.MediaLibary
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
                (DataContext as ViewModels.MediaLibary.MediaLibaryViewModel)?.CategoryQueryCommand.Execute(new string[] { selected, tag.ToString() });
            }

        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selected = DataTree.SelectedItem as string;
            var tag = (e.OriginalSource as TextBlock)?.Tag;
            if (selected != null && tag != null)
            {
                (DataContext as ViewModels.MediaLibary.MediaLibaryViewModel)?.DeleteQueryCommand.Execute(new string[] { selected, tag.ToString() });
            }
        }
    }
}
