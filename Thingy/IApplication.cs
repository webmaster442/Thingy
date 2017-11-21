using AppLib.WPF.MVVM;
using System.Windows.Controls;

namespace Thingy
{
    public interface IApplication
    {
        bool? ShowDialog(UserControl control, string Title, ViewModel model = null);
        void SetCurrentTabContent(string Title, UserControl control);
        void Close();
    }
}
