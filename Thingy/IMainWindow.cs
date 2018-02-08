using AppLib.MVVM;
using System.Windows.Controls;

namespace Thingy
{
    public interface IMainWindow: IView
    {
        void ShowHideMenu();
        UserControl CurrentTabContent { get; }
    }
}
