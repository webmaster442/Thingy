using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.Infrastructure;

namespace Thingy
{
    public class TabManager : ITabManager
    {
        private MainWindow MainWindow
        {
            get { return Application.Current.MainWindow as MainWindow; }
        }

        private IModuleLoader _moduleLoader;
        private IApplication _application;

        public TabManager(IApplication application, IModuleLoader moduleLoader)
        {
            _application = application;
            _moduleLoader = moduleLoader;
        }

        public void CreateNewTabContent(string Title, UserControl control)
        {
            MainWindow.SetCurrentTabContent(Title, control, true);
        }

        public void FocusTabByIndex(int index)
        {
            MainWindow.FocusTabByIndex(index);
        }

        public int GetTabIndexByTitle(string Title)
        {
            return MainWindow.FindTabByTitle(Title);
        }

        public void SetCurrentTabContent(string Title, UserControl control)
        {
            MainWindow.SetCurrentTabContent(Title, control, false);
        }

        public async Task StartModule(IModule module)
        {
            if (module == null) return;
            var control = module.RunModule();
            if (control != null)
            {
                if (module.OpenAsWindow)
                {
                    await _application.ShowDialog(control, module.ModuleName);
                }
                else
                {
                    if (module.IsSingleInstance)
                    {
                        var index = GetTabIndexByTitle(module.ModuleName);
                        if (index != -1)
                        {
                            await _application.ShowMessageBox("Single instance module",
                                                              "This module can only run in one instance. Switching to running module",
                                                              MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                            FocusTabByIndex(index);
                        }
                        else
                            SetCurrentTabContent(module.ModuleName, control);
                    }
                    else
                        SetCurrentTabContent(module.ModuleName, control);
                }
            }
        }
    }
}
