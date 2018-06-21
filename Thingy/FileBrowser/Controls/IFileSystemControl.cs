using System;
using System.Collections.Generic;

namespace Thingy.FileBrowser.Controls
{
    interface IFileSystemControl
    {
        string SelectedPath { get; set; }
        bool IsHiddenVisible { get; set; }
        event EventHandler<string> OnNavigationException;
    }

    interface IFileDisplayControl: IFileSystemControl
    {
        IEnumerable<string> FilteredExtensions { get; set; }
        void GoHome();
    }
}
