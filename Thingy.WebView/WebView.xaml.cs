using System.Windows.Controls;
using Thingy.Resources;

namespace Thingy.WebView
{
    /// <summary>
    /// Interaction logic for WebView.xaml
    /// </summary>
    public partial class WebViewControl : UserControl, IWebViewControl
    {
        public WebViewControl()
        {
            InitializeComponent();
        }

        public void LoadEmbededMarkdown(string url)
        {
            var content = ResourceLocator.GetResourceFile(url);
            LoadMarkdown(content);
        }

        public void LoadHtmlText(string htmlContent)
        {
            WebBrowser.NavigateToString(htmlContent);
        }

        public void LoadMarkdown(string Markdown)
        {
            var html = Markdown2Html.RenderMarkdown2Html(Markdown);
            WebBrowser.NavigateToString(html);
        }
    }
}
