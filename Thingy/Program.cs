using System;
using System.Windows;
using Thingy.Infrastructure;
using AppLib.MVVM.IoC;
using Thingy.Implementation;
using Thingy.API;
using Thingy.Db;
using Thingy.InternalCode;

namespace Thingy
{
    public class Program
    {
        public static CommandLineParser CommandLineParser { get; private set; }
        public static IoCContainer Resolver { get; private set; }
        public static IntPtr WinSparklePtr;

        private static ILog _log;
        private static ISettings _settings;
        private static IDataBase _db;
        private static IModuleLoader _moduleLoader;

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

        private static void SetupAutoUpdater()
        {
            WinSparkle.SetDllDirectory(Paths.Resolve(Paths.NativeDllPath));
            WinSparkle.win_sparkle_set_appcast_url(AppConstants.AutoUpdateURL);
            WinSparkle.win_sparkle_init();
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
            application.MainWindow = new MainWindow(application);
            return application;
        }

        private static void AppShutDown(AppLib.Common.SingleInstanceApp singleInstance)
        {
            singleInstance.Close();
            _settings.Save();
            _log.Info("Application shutdown");
            _log.WriteToFile();
            WinSparkle.win_sparkle_cleanup();
        }

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
                application.Run(application.MainWindow);
                AppShutDown(singleInstance);
            }
            else singleInstance.SubmitParameters();
        }

        private static void CommandLineArgumentsRecieved(string obj)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                CommandLineParser.Parse(obj);
            });
        }
    }
}
