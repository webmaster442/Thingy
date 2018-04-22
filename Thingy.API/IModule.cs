using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Thingy.API
{
    public class OpenParameters
    {
        public DialogButtons DialogButtons { get; set; }
    }

    public interface IModule
    {
        IApplication App { get; set; }
        string ModuleName { get; }
        ImageSource Icon { get; }
        UserControl RunModule();
        Color TileColor { get; }
        SolidColorBrush ColorBrush { get; }
        bool CanLoad { get; }
        string Category { get; }
        OpenParameters OpenParameters { get; }
        bool IsSingleInstance { get; }
        IEnumerable<string> SupportedExtensions { get; }
        void AppAttached();
    }
}
