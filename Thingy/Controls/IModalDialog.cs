using System.Windows.Controls;
using Thingy.API;

namespace Thingy.Controls
{
    public interface IModalDialog
    {
        object DailogContent { get; set; }
        DialogButtons DialogButtons { get; set; }
    }
}