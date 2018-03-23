using System.Windows;
using System.Windows.Controls;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore.Controls
{
    /// <summary>
    /// Interaction logic for TrackInfo.xaml
    /// </summary>
    public partial class TrackInfo : UserControl
    {
        public TrackInfo()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(TagInformation), typeof(TrackInfo), new PropertyMetadata(null, TagChanged));

        private static void TagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TrackInfo control)
            {
                control.Dispatcher.Invoke(() =>
                {
                    var tags = e.NewValue as TagInformation;
                    if (tags == null)
                    {
                        control.Artist.Text = "";
                        control.Title.Text = "";
                        control.Year.Text = "";
                        control.Album.Text = "";
                        control.Filename.Text = "";
                        control.Cover.Source = null;
                    }
                    else
                    {
                        control.Artist.Text = tags.Artist;
                        control.Title.Text = tags.Title;
                        control.Year.Text = tags.Year;
                        control.Album.Text = tags.Album;
                        control.Filename.Text = tags.FileName;
                        control.Cover.Source = tags.Cover;
                    }
                });
            }
        }

        public TagInformation Tags
        {
            get { return (TagInformation)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }
    }
}
