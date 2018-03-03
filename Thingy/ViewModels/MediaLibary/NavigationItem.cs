using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Thingy.ViewModels.MediaLibary
{
    public class NavigationItem
    {
        public string Name { get; set; }
        public ImageSource Icon { get; set; }
        public ObservableCollection<string> SubItems { get; set; }
    }
}
