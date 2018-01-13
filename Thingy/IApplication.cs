using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Thingy
{
    public interface IApplication
    {
        Task<bool> ShowDialog(UserControl control, string Title, INotifyPropertyChanged model = null);
        Task<MessageDialogResult> ShowMessageBox(string title, string content, MessageDialogStyle style);
        void ShowStatusBarMenu(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000);
        void SetCurrentTabContent(string Title, UserControl control);
        void OpenTabContent(string Title, UserControl control);
        int FindTabByTitle(string Title);
        void FocusTabByIndex(int index);
        void Close();
    }
}
