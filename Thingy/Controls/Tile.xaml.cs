using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        public static readonly DependencyProperty TileTextProperty =
            DependencyProperty.Register("TileText", typeof(string), typeof(Tile), new PropertyMetadata("Tile Text"));

        public static readonly DependencyProperty TileImageProperty =
            DependencyProperty.Register("TileImage", typeof(ImageSource), typeof(Tile));

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(Tile), new PropertyMetadata(null));

        public string TileText
        {
            get { return (string)GetValue(TileTextProperty); }
            set { SetValue(TileTextProperty, value); }
        }

        public ImageSource TileImage
        {
            get { return (ImageSource)GetValue(TileImageProperty); }
            set { SetValue(TileImageProperty, value); }
        }

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public Tile()
        {
            InitializeComponent();
        }
    }
}
