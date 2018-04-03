using System;
using System.Windows;
using Thingy.Infrastructure;
using AppLib.MVVM.IoC;
using Thingy.Implementation;
using Thingy.API;
using Thingy.Db;

namespace Thingy
{
    public class Program
    {
        public static CommandLineParser CommandLineParser { get; private set; }
        public static IoCContainer Resolver { get; private set; }

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
            _moduleLoader = new ModuleLoader(_log);

            Resolver.Register<ILog>(() => _log);
            Resolver.Register<ISettings>(() => _settings);
            Resolver.Register<IDataBase>(() => _db);
            Resolver.Register<IModuleLoader>(() => _moduleLoader);
        }

        [STAThread]
        public static void Main()
        {
            SetupIoCContainer();
            const string appName = "Thingy";
            var singleInstance = new AppLib.Common.SingleInstanceApp(appName);
            singleInstance.CommandLineArgumentsRecieved += CommandLineArgumentsRecieved;
            if (singleInstance.IsFirstInstance)
            {
                var application = new App();
                Resolver.Register<IApplication>(() => application);
                CommandLineParser = new CommandLineParser(application);
                JumpListFactory.CreateJumplist();
                application.InitializeComponent();
                application.ShutdownMode = ShutdownMode.OnMainWindowClose;
                application.MainWindow = new MainWindow(application);
                application.Run(application.MainWindow);
                singleInstance.Close();

                _settings.Save();
                _log.Info("Application shutdown");
                _log.WriteToFile();
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
