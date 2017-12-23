using System.ComponentModel;
using System.Windows.Controls;

namespace Thingy
{
    public interface IApplication
    {
        bool? ShowDialog(UserControl control, string Title, INotifyPropertyChanged model = null);
        void SetCurrentTabContent(string Title, UserControl control);
        void OpenTabContent(string Title, UserControl control);
        int FindTabByTitle(string Title);
        void FocusTabByIndex(int index);
        void Close();
    }
}
