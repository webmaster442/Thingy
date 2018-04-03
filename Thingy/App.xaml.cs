using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.API;
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

        public void Close()
        {
            Current.Shutdown();
        }

        public Task CloseMessageBox(CustomDialog messageBoxContent)
        {
            var mainwindow = (Current.MainWindow as MainWindow);
            return mainwindow.ShowMetroDialogAsync(messageBoxContent);
        }

        public async void HandleFiles(IList<string> files)
        {
            var loader = Resolve<IModuleLoader>();
            foreach (var file in files)
            {
                var module = loader.GetModuleForFile(file);
                if (module == null) continue;

                if (module.IsSingleInstance)
                {
                    int tabIndex = TabManager.GetTabIndexByTitle(module.ModuleName);
                    if (tabIndex == -1)
                    {
                        var id = await TabManager.StartModule(module);
                        await Task.Delay(25);
                        Messager.SendMessage(id, new HandleFileMessage(Guids.Application, file));
                    }
                    else
                    {
                        TabManager.FocusTabByIndex(tabIndex);
                        Messager.SendMessage(module.RunModule().GetType(), new HandleFileMessage(Guids.Application, file));
                    }
                }
                else
                {
                    var id = await TabManager.StartModule(module);
                    await Task.Delay(25);
                    Messager.SendMessage(id, new HandleFileMessage(Guids.Application, file));
                }
            }
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
            return mainwindow.HideMetroDialogAsync(messageBoxContent);
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

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            var accent = Settings.Get(SettingsKeys.ProgramAccent, "Orange");

            Messager = new Messager();
            TabManager = new TabManager(this, Resolve<IModuleLoader>());

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
