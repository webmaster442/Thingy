namespace Thingy.WebView
{
    public interface IWebViewControl
    {
        void LoadHtmlText(string htmlContent);
        void LoadMarkdown(string Markdown);
        void LoadEmbededMarkdown(string url);
    }
}
