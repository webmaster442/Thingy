using System.Windows.Controls;

namespace Thingy.Views.MusicPlayer
{
    /// <summary>
    /// Interaction logic for AddURLDialog.xaml
    /// </summary>
    public partial class AddURLDialog : UserControl
    {
        public AddURLDialog()
        {
            InitializeComponent();
        }

        public string Url
        {
            get { return TbUrl.Text; }
            set { TbUrl.Text = value; }
        }
    }
}
