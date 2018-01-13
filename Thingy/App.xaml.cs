using AppLib.WPF;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.Db;
using Thingy.Infrastructure;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication, IDisposable
    {
        public static AppLib.Common.IOC.IContainer IoCContainer { get; private set; }
        public static AppLib.Common.Log.ILogger Log { get; private set; }

        public static string[] Accents { get; private set; }

        public static IApplication Instance
        {
            get { return App.Current as IApplication; }
        }

        public void Close()
        {
            App.Current.Shutdown();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int FindTabByTitle(string Title)
        {
            return (App.Current.MainWindow as MainWindow).FindTabByTitle(Title);
        }

        public void FocusTabByIndex(int index)
        {
            (App.Current.MainWindow as MainWindow).FocusTabByIndex(index);
        }

        public void OpenTabContent(string Title, UserControl control)
        {
            (App.Current.MainWindow as MainWindow).SetCurrentTabContent(Title, control, true);
        }

        public void SetCurrentTabContent(string Title, UserControl control)
        {
            (App.Current.MainWindow as MainWindow).SetCurrentTabContent(Title, control, false);
        }

        public async Task<bool> ShowDialog(UserControl control, string Title, INotifyPropertyChanged model = null)
        {
            ModalDialog modalDialog = new ModalDialog();
            if (model != null)
                control.DataContext = model;
            modalDialog.DailogContent = control;
            modalDialog.Title = Title;

            var result = await (App.Current.MainWindow as MainWindow).ShowChildWindowAsync<bool>(modalDialog);

            return result;
        }

        public Task<MessageDialogResult> ShowMessageBox(string title, string content, MessageDialogStyle style)
        {
            var mainwindow = (App.Current.MainWindow as MainWindow);
            return mainwindow.ShowMessageAsync(title, content, style);
        }

        public void ShowStatusBarMenu(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000)
        {
            var mainwindow = (App.Current.MainWindow as MainWindow);
            mainwindow.StatusFlyOut.Content = control;
            mainwindow.StatusFlyOut.AutoCloseInterval = AutoCloseTimeMs;
            mainwindow.StatusFlyOut.IsAutoCloseEnabled = AutoClose;
            mainwindow.StatusFlyOut.Header = title;
            mainwindow.StatusFlyOut.IsOpen = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Log = new AppLib.Common.Log.Logger();
            Log.Info("Application startup");


            var trayIcon = new Infrastructure.Tray.TrayIcon();

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            Accents = new string[]
            {
                "Red", "Green", "Blue",
                "Purple", "Orange", "Lime",
                "Emerald", "Teal", "Cyan",
                "Cobalt", "Indigo", "Violet",
                "Pink", "Magenta", "Crimson",
                "Amber", "Yellow", "Brown",
                "Olive", "Steel", "Mauve",
                "Taupe", "Sienna"
            };
            IoCContainer = new AppLib.Common.IOC.Container();
            IoCContainer.RegisterSingleton<IDataBase>(() =>
            {
                return new DataBase("test.db");
            });
            Log.Info("Database initialized");
            IoCContainer.RegisterSingleton<IModuleLoader>(() =>
            {
                return new ModuleLoader(Log);
            });

            var accent = Thingy.Properties.Settings.Default.SelectedAccent;
            ThemeManager.ChangeAppStyle(Application.Current,
                              ThemeManager.GetAccent(accent),
                              ThemeManager.GetAppTheme("BaseLight"));

            var dload = BingPhotoOfDay.WasSuccesfull;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            var desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            var log = System.IO.Path.Combine(desktop, "Thingy crash log.xml");
            Log.SaveToFile(log);
            AppLib.WPF.Dialogs.Dialogs.ShowErrorDialog(e.Exception);
        }
    }
}
