using AppLib.MVVM;
using CommonMark;
using System.IO;
using System.Reflection;
using System.Text;

namespace Thingy.ViewModels
{
    public class NoteViewModel : ViewModel
    {
        private string _MarkDownText;
        private string _RenderedText;
        private string _Template;

        public NoteViewModel()
        {
            var executing = Assembly.GetExecutingAssembly();
            using (Stream stream = executing.GetManifestResourceStream("Thingy.Resources.MarkdownTemplate.html"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    _Template = reader.ReadToEnd();
                }
            }
        }

        public string Combine(string str)
        {
            StringBuilder output = new StringBuilder();
            output.Append(_Template);
            output.Append(str);
            output.Append("</body></html>");
            return output.ToString();
        }

        public string MarkDownText
        {
            get { return _MarkDownText; }
            set
            {
                if (SetValue(ref _MarkDownText, value))
                {
                    try
                    {
                        RenderedText = Combine(CommonMarkConverter.Convert(_MarkDownText));
                    }
                    catch (CommonMarkException ex)
                    {
                        RenderedText = Combine($"<pre>Render error:\r\n{ex.Message}</pre>");
                    }
                }
            }
        }

        public string RenderedText
        {
            get { return _RenderedText; }
            set { SetValue(ref _RenderedText, value); }
        }
    }
}
