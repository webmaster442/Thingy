using AppLib.WPF.MVVM;
using System.Windows.Controls;

namespace Thingy
{
    public interface IApplication
    {
        bool? ShowDialog(UserControl control, ViewModel model = null);
        void Close();
    }
}
