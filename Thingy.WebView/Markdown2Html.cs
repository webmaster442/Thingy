using CommonMark;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Thingy.WebView
{
    internal static class Markdown2Html
    {
        private static Assembly _current;
        private static string[] _resources;
        private static string _dependencyCache;

        static Markdown2Html()
        {
            _current = Assembly.GetAssembly(typeof(Markdown2Html));
            _resources = _current.GetManifestResourceNames();
        }

        private static string GetResourceContent(string search)
        {
            var fullName = _resources.Where(r => r.StartsWith(search)).FirstOrDefault();

            string result = string.Empty;

            if (fullName == null) return result;

            using (var stream = _current.GetManifestResourceStream(fullName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        private static string GetDependencies()
        {
            if (_dependencyCache != null) return _dependencyCache;

            StringBuilder dependencies = new StringBuilder();

            dependencies.AppendFormat("<script type=\"text/javascript\">{0}</script>", GetResourceContent("jquery-1.12.4.min.js"));
            dependencies.AppendFormat("<script type=\"text/javascript\">{0}</script>", GetResourceContent("toc.js"));
            dependencies.AppendFormat("<style type=\"text/css\">{0}</style>", GetResourceContent("style.css"));

            _dependencyCache = dependencies.ToString();

            return _dependencyCache;
        }

        public static string RenderMarkdown2Html(string Markdown)
        {
            try
            {
                var md = CommonMarkConverter.Convert(Markdown);

                var template = GetResourceContent("template.html");
                template.Replace("[dependencies]", GetDependencies());
                template.Replace("[content]", md);

                return template;
            }
            catch (CommonMarkException ex)
            {
                return $"<pre>Render error:\r\n{ex.Message}</pre>";
            }
        }
    }
}
