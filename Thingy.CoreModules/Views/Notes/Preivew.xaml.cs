using System.Windows.Controls;

namespace Thingy.CoreModules.Views.Notes
{
    /// <summary>
    /// Interaction logic for Preivew.xaml
    /// </summary>
    public partial class Preivew : UserControl
    {
        public Preivew()
        {
            InitializeComponent();
        }

        public void SetContent(string s)
        {
            Document.Text = s;
        }
    }
}
