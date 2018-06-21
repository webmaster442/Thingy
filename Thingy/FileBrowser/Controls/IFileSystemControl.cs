using System.Collections.Generic;

namespace Thingy.FileBrowser.Controls
{
    interface IFileSystemControl
    {
        string SelectedPath { get; set; }
        bool IsHiddenVisible { get; set; }
    }

    interface IFileDisplayControl: IFileSystemControl
    {
        IEnumerable<string> FilteredExtensions { get; set; }
    }
}
