using AppLib.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.Infrastructure
{
    public class TabManager : ITabManager
    {
        private Dictionary<UId, IModule> _runningModules;
        private IModuleLoader _moduleLoader;
        private IApplication _application;
        private Random _random;

        private MainWindow MainWindow
        {
            get { return System.Windows.Application.Current.MainWindow as MainWindow; }
        }

        public TabManager(IApplication application, IModuleLoader moduleLoader)
        {
            _application = application;
            _moduleLoader = moduleLoader;
            _runningModules = new Dictionary<UId, IModule>();
            _random = new Random();
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

        public async Task<UId> StartModule(IModule module)
        {
            if (module == null) return null;
            var control = module.RunModule();
            if (control != null)
            {
                if (module.OpenAsWindow)
                {
                    bool result = await _application.ShowDialog(control, module.ModuleName);
                    if (result && control is IHaveCloseTask closetask)
                    {
                        await closetask.ClosingTask();
                    }
                    return null;
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
                            return null;
                        }
                        else
                        {
                            var mId = new UId();
                            _runningModules.Add(mId, module);
                            control.Tag = mId;
                            SetCurrentTabContent(module.ModuleName, control);
                            return mId;
                        }
                    }
                    else
                    {
                        var mId = new UId();
                        _runningModules.Add(mId, module);
                        control.Tag = mId;
                        SetCurrentTabContent(module.ModuleName, control);
                        return mId;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public void ModuleClosed(UId ModuleId)
        {
            _runningModules.Remove(ModuleId);
        }
    }
}
