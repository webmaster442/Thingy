using AppLib.MVVM.IoC;
using System;
using System.Windows;
using Thingy.API;
using Thingy.Db;
using Thingy.Implementation;
using Thingy.Infrastructure;
using Thingy.InternalCode;

namespace Thingy
{
    public class Program
    {
        private static IDataBase _db;
        private static ILog _log;
        private static IModuleLoader _moduleLoader;
        private static ISettings _settings;

        private static void AppShutDown(AppLib.Common.SingleInstanceApp singleInstance)
        {
            singleInstance.Close();
            _settings.Save();
            _log.Info("Application shutdown");
            _log.WriteToFile();
            WinSparkle.win_sparkle_cleanup();
        }

        private static void CommandLineArgumentsRecieved(string obj)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                CommandLineParser.Parse(obj);
            });
        }

        private static App SetupApplication()
        {
            var application = new App();
            _moduleLoader = new ModuleLoader(application);
            Resolver.Register<IApplication>(() => application);
            Resolver.Register<IModuleLoader>(() => _moduleLoader);
            CommandLineParser = new CommandLineParser(application);
            JumpListFactory.CreateJumplist();
            application.InitializeComponent();
            application.ShutdownMode = ShutdownMode.OnMainWindowClose;
            application.MainWindow = new MainWindow(application, _moduleLoader);
            return application;
        }

        private static void SetupAutoUpdater()
        {
            WinSparkle.SetDllDirectory(Paths.Resolve(Paths.NativeDllPath));
            WinSparkle.win_sparkle_set_appcast_url(AppConstants.AutoUpdateURL);
            WinSparkle.win_sparkle_init();
        }

        private static void SetupIoCContainer()
        {
            Resolver = new IoCContainer();

            _log = new Log(Paths.Resolve(Paths.LogPath));
            _log.BigDivider();
            _log.Info("Application startup");

            _settings = new Settings(_log);
            _db = new DataBase(Paths.Resolve(Paths.DBPath));
            API.SettingsBinding.Settings = _settings;

            Resolver.Register<ILog>(() => _log);
            Resolver.Register<ISettings>(() => _settings);
            Resolver.Register<IDataBase>(() => _db);
        }

        public static IntPtr WinSparklePtr;
        public static CommandLineParser CommandLineParser { get; private set; }
        public static IoCContainer Resolver { get; private set; }

        [STAThread]
        public static void Main()
        {
            SetupAutoUpdater();
            SetupIoCContainer();
            var singleInstance = new AppLib.Common.SingleInstanceApp(AppConstants.AppName);
            singleInstance.CommandLineArgumentsRecieved += CommandLineArgumentsRecieved;
            if (singleInstance.IsFirstInstance)
            {
                App application = SetupApplication();
                _moduleLoader.Add(new FileBrowser.FileBrowserModule());
                application.Run(application.MainWindow);
                AppShutDown(singleInstance);
            }
            else singleInstance.SubmitParameters();
        }
    }
}
