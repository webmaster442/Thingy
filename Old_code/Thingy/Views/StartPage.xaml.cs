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
            var brush = BingPhotoOfDay.PhotoOfDayImageBrush;
            brush.TileMode = System.Windows.Media.TileMode.None;
            ItemList.Background = brush;

        }
    }
}
