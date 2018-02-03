using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Thingy.Infrastructure;

namespace Thingy
{
    public class MainClass
    {
        public static CommandLineParser CommandLineParser { get; private set; }

        [STAThread]
        public static void Main()
        {
            const string appName = "Thingy";
            var singleInstance = new AppLib.Common.SingleInstanceApp(appName);
            singleInstance.CommandLineArgumentsRecieved += CommandLineArgumentsRecieved;
            if (singleInstance.IsFirstInstance)
            {
                var application = new App();
                CommandLineParser = new CommandLineParser(application);
                JumpListFactory.CreateJumplist();
                application.InitializeComponent();
                application.ShutdownMode = ShutdownMode.OnMainWindowClose;
                application.Run();
                singleInstance.Close();
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
