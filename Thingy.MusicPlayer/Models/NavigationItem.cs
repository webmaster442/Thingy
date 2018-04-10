using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Thingy.MediaLibary.Models
{
    public class NavigationItem
    {
        public string Name { get; set; }
        public ImageSource Icon { get; set; }
        public ObservableCollection<string> SubItems { get; set; }
    }
}
