using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.Infrastructure;

namespace Thingy
{
    public class TabManager : ITabManager
    {
        private Dictionary<int, IModule> _runningModules;
        private IModuleLoader _moduleLoader;
        private IApplication _application;
        private Random _random;

        private MainWindow MainWindow
        {
            get { return Application.Current.MainWindow as MainWindow; }
        }

        public TabManager(IApplication application, IModuleLoader moduleLoader)
        {
            _application = application;
            _moduleLoader = moduleLoader;
            _runningModules = new Dictionary<int, IModule>();
            _random = new Random();
        }

        private int GenerateModuleId()
        {
            int id = 0;
            do { id = _random.Next(int.MinValue, int.MaxValue); }
            while (_runningModules.ContainsKey(id));
            return id;
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
                        {
                            var mId = GenerateModuleId();
                            _runningModules.Add(mId, module);
                            control.Tag = mId;
                            SetCurrentTabContent(module.ModuleName, control);
                        }
                    }
                    else
                    {
                        var mId = GenerateModuleId();
                        _runningModules.Add(mId, module);
                        control.Tag = mId;
                        SetCurrentTabContent(module.ModuleName, control);
                    }
                }
            }
        }

        public void ModuleClosed(int ModuleId)
        {
            _runningModules.Remove(ModuleId);
        }
    }
}
