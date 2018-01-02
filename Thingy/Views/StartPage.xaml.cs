using System.Windows.Controls;
using AppLib.WPF;

namespace Thingy.Views
{

    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
            ItemList.Background = BingPhotoOfDay.PhotoOfDayImageBrush;
        }
    }
}
