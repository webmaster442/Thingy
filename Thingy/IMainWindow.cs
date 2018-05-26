using AppLib.MVVM;
using System.Windows.Controls;

namespace Thingy
{
    public interface IMainWindow: IView
    {
        void OpenOrHideFlyout(string flyoutName);
        UserControl CurrentTabContent { get; }
        void SetBusyOverlayVisibility(bool isVisible);
    }
}
