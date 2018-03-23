using System.Windows.Controls;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.WPF;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
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

        private void AboutView_LinkClicked(object sender, RoutedEvenArgs<HtmlLinkClickedEventArgs> args)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = args.Data.Link;
            p.Start();
            args.Handled = true;
        }
    }
}
