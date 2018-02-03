using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Thingy.Infrastructure;

namespace Thingy
{
    public interface IApplication
    {
        Task<bool> ShowDialog(UserControl control, string Title, INotifyPropertyChanged model = null);
        Task<MessageDialogResult> ShowMessageBox(string title, string content, MessageDialogStyle style);
        Task ShowMessageBox(CustomDialog messageBoxContent);
        Task HideMessageBox(CustomDialog messageBoxContent);
        void ShowStatusBarMenu(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000);
        ITabManager TabManager { get; }
        void Close();
        void Restart();
    }

    public interface ITabManager
    {
        void SetCurrentTabContent(string Title, UserControl control);
        void CreateNewTabContent(string Title, UserControl control);
        int GetTabIndexByTitle(string Title);
        void FocusTabByIndex(int index);
        Task StartModule(IModule module);
        void ModuleClosed(int ModuleId);
    }
}
