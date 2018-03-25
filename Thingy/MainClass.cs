using System;
using System.Windows;
using Thingy.Infrastructure;
using AppLib.MVVM.IoC;
using Thingy.Implementation;
using Thingy.API;

namespace Thingy
{
    public class Program
    {
        public static CommandLineParser CommandLineParser { get; private set; }
        public static IoCContainer Resolver { get; private set; }

        private static ILog _log;
        private static ISettings _settings;

        private static void SetupIoCContainer()
        {
            Resolver = new IoCContainer();
            _log = new Log(Paths.Resolve(Paths.LogPath));
            _settings = new Settings(_log);
            Resolver.Register<ILog>(() => _log);
            Resolver.Register<ISettings>(() => _settings);
        }

        [STAThread]
        public static void Main()
        {
            SetupIoCContainer();

            _log.BigDivider();
            _log.Info("Application startup");

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
                application.Run();
                singleInstance.Close();
                _settings.Save();
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
