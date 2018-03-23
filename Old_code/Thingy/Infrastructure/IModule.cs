using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Thingy.Infrastructure
{
    public interface IModule
    {
        string ModuleName { get; }
        ImageSource Icon { get; }
        UserControl RunModule();
        Color TileColor { get; }
        SolidColorBrush ColorBrush { get; }
        bool CanLoad { get; }
        string Category { get; }
        bool OpenAsWindow { get; }
        bool IsSingleInstance { get; }
        IEnumerable<string> SupportedExtensions { get; }
    }
}
