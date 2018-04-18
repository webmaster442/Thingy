using System.Windows.Controls;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.WPF;

namespace Thingy.InternalModules
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    internal partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            AboutView.Text = RaeadText();
        }

        private string RaeadText()
        {
            return Thingy.Resources.ResourceLocator.GetResourceFile("html.AboutPage.html");
        }
    }
}
