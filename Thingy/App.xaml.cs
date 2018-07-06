using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.API;
using Thingy.API.Jobs;
using Thingy.API.Messages;
using Thingy.Controls;
using Thingy.Implementation;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication
    {
        #region Interface Implementations

        public ISettings Settings
        {
            get { return Program.Resolver.Resolve<ISettings>(); }
        }

        public ILog Log
        {
            get { return Program.Resolver.Resolve<ILog>(); }
        }

        public IMessager Messager
        {
            get;
            private set;
        }

        public ITabManager TabManager
        {
            get;
            private set;
        }

        public Dispatcher CurrentDispatcher
        {
            get { return Current.Dispatcher; }
        }

        public IJobRunner JobRunner
        {
            get;
            private set;
        }

        public void Close()
        {
            Current.Shutdown();
        }

        public Task CloseMessageBox(CustomDialog messageBoxContent)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            return mainwindow.ShowMetroDialogAsync(messageBoxContent);
        }

        public async void HandleFiles(string preferedModuleName, IList<string> files)
        {
            var loader = Resolve<IModuleLoader>();
            var module = loader.GetModuleByName(preferedModuleName);
            await SendFilesToModule(files, module);
        }

        public async void HandleFiles(IList<string> files)
        {
            var loader = Resolve<IModuleLoader>();

            IModule module = null;
            var modules = loader.GetModulesForFiles(files);
            if (modules == null || modules.Count == 0) return;

            else if (modules.Count == 1)
            {
                module = modules[0];
            }
            else
            {
                module = await SelectModule(modules);
            }

            await SendFilesToModule(files, module);
        }

        private async Task SendFilesToModule(IList<string> files, IModule module)
        {
            if (module == null) return;

            var msgcontent = SelectFilesSupportedByModule(files, module);

            int tabIndex = TabManager.GetTabIndexByTitle(module.ModuleName);
            if (tabIndex == -1)
            {
                var id = await TabManager.StartModule(module, true);
                await Task.Delay(25);
                Messager.SendMessage(id, new HandleFileMessage(AppConstants.ApplicationGuid, msgcontent));
            }
            else
            {
                TabManager.FocusTabByIndex(tabIndex);
                Messager.SendMessage(module.RunModule().GetType(), new HandleFileMessage(AppConstants.ApplicationGuid, msgcontent));
            }
        }

        private IEnumerable<string> SelectFilesSupportedByModule(IEnumerable<string> files, IModule module)
        {
            return
                from file in files
                where module.CanHadleFile(file)
                select file;
        }

        private async Task<IModule> SelectModule(IList<IModule> modules)
        {
            OpenSelector selector = new OpenSelector(this);
            selector.InputModules = modules;
            if (await ShowDialog("Select module", selector, DialogButtons.OkCancel))
                return selector.SelectedModule;
            return null;
        }

        public Task HideMessageBox(CustomDialog messageBoxContent)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            return mainwindow.HideMetroDialogAsync(messageBoxContent);
        }

        public void Register<T>(Func<T> getter)
        {
            Program.Resolver.Register(getter);
        }

        public bool CanResolve<T>()
        {
            return Program.Resolver.CanResolve<T>();
        }

        public T Resolve<T>()
        {
            return Program.Resolver.Resolve<T>();
        }

        public void Restart()
        {
            System.Diagnostics.Process.Start(ResourceAssembly.Location);
            Current.Shutdown();
        }

        public async Task<bool> ShowDialog(string title, UserControl content, DialogButtons buttons, bool hasShadow = true, INotifyPropertyChanged modell = null)
        {
            ModalDialog modalDialog = new ModalDialog();

            if (hasShadow == false)
                modalDialog.OverlayBrush = null;

            if (modell != null)
                content.DataContext = modell;

            modalDialog.DailogContent = content;
            modalDialog.Title = title;
            modalDialog.DialogButtons = buttons;

            var result = await (Current.MainWindow as MainWindow).ShowChildWindowAsync<bool>(modalDialog);

            return result;
        }

        public bool ShowRealDialog(string title, UserControl content, DialogButtons buttons, INotifyPropertyChanged modell = null)
        {
            ModalDialogReal modalDialog = new ModalDialogReal();

            if (modell != null)
                content.DataContext = modell;

            modalDialog.DailogContent = content;
            modalDialog.Title = title;
            modalDialog.DialogButtons = buttons;

            var result = modalDialog.ShowDialog();

            return result.Value;
        }

        public async Task<bool> ShowMessageBox(string title, string content, DialogButtons buttons)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            var settings = new MetroDialogSettings();

            MessageDialogResult result;

            switch (buttons)
            {
                case DialogButtons.Ok:
                case DialogButtons.None:
                    {
                        settings.AffirmativeButtonText = "Ok";
                        result = await mainwindow.ShowMessageAsync(title, content, MessageDialogStyle.Affirmative, settings);
                    }
                    break;
                case DialogButtons.YesNo:
                    {
                        settings.AffirmativeButtonText = "Yes";
                        settings.NegativeButtonText = "No";
                        result = await mainwindow.ShowMessageAsync(title, content, MessageDialogStyle.AffirmativeAndNegative, settings);
                    }
                    break;
                default:
                case DialogButtons.OkCancel:
                    result = await mainwindow.ShowMessageAsync(title, content, MessageDialogStyle.AffirmativeAndNegative);
                    break;
            }

            switch (result)
            {
                case MessageDialogResult.Affirmative:
                    return true;
                default:
                    return false;
            }
        }

        public Task ShowMessageBox(CustomDialog messageBoxContent)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            return mainwindow.ShowMetroDialogAsync(messageBoxContent);
        }

        public void ShowStatusBarMenu(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            mainwindow.StatusFlyOut.Content = control;
            mainwindow.StatusFlyOut.AutoCloseInterval = AutoCloseTimeMs;
            mainwindow.StatusFlyOut.IsAutoCloseEnabled = AutoClose;
            mainwindow.StatusFlyOut.Header = title;
            mainwindow.StatusFlyOut.IsOpen = true;
        }

        public void ShowFlyoutLeft(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            mainwindow.LeftFlyOut.Content = control;
            mainwindow.LeftFlyOut.AutoCloseInterval = AutoCloseTimeMs;
            mainwindow.LeftFlyOut.IsAutoCloseEnabled = AutoClose;
            mainwindow.LeftFlyOut.Header = title;
            mainwindow.LeftFlyOut.IsOpen = true;
        }

        #endregion

        public App() : base()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            var accent = Settings.Get(SettingsKeys.ProgramAccent, "Orange");

            Messager = new Messager();
            TabManager = new TabManager(this, Resolve<IModuleLoader>());
            JobRunner = new JobRunner(this);

            var trayIcon = new Implementation.Tray.TrayIcon(this);

            ThemeManager.ChangeAppStyle(Current,
                              ThemeManager.GetAccent(accent),
                              ThemeManager.GetAppTheme("BaseLight"));

        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Divider();
            Log.Error("Fatal unhandled exception");
            Log.Error(e.Exception);
            Log.WriteToFile();
            AppLib.WPF.Dialogs.Dialogs.ShowErrorDialog(e.Exception);
        }
    }
}
