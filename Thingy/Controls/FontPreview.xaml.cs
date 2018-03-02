using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for FontPreview.xaml
    /// </summary>
    public partial class FontPreview : UserControl
    {
        public FontPreview()
        {
            InitializeComponent();
        }

        private void Rb_Click(object sender, RoutedEventArgs e)
        {
            string source = ((RadioButton)sender).Name;
            switch (source)
            {
                case "RbEnglish":
                    InputText.Text = "The quick brown fox jumps over the lazy dog";
                    break;
                case "RbHun":
                    InputText.Text = "Árvíztűrő tükörfúrógép";
                    break;
            }
        }

        public static DependencyProperty PreviewFontFamilyProperty =
            DependencyProperty.Register("PreviewFontFamily", typeof(FontFamily), typeof(FontPreview));

        public FontFamily PreviewFontFamily
        {
            get { return (FontFamily)GetValue(PreviewFontFamilyProperty); }
            set { SetValue(PreviewFontFamilyProperty, value); }
        }
    }
}
