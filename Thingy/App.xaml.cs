using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;
using Thingy.API.Messages;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication
    {
        public ISettings Settings => throw new NotImplementedException();

        public ILog Log
        {
            get { return Program.Resolver.Resolve<ILog>(); }
        }

        public ITypeResolver Resolver => throw new NotImplementedException();

        public IMessager Messager => throw new NotImplementedException();

        public ITabManager TabManager => throw new NotImplementedException();

        public void Close()
        {
            Current.Shutdown();
        }

        public Task CloseMessageBox(CustomDialog messageBoxContent)
        {
            throw new NotImplementedException();
        }

        public void HandleFiles(IList<string> files)
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            System.Diagnostics.Process.Start(ResourceAssembly.Location);
            Current.Shutdown();
        }

        public Task<bool> ShowDialog(string title, UserControl content, DialogButtons buttons, bool hasShadow = true, INotifyPropertyChanged modell = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowMessageBox(string title, string content, DialogButtons buttons)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessageBox(CustomDialog messageBoxContent)
        {
            throw new NotImplementedException();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Info("Application startup");
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("Fatal unhandled exception");
            Log.Error(e.Exception);
            Log.WriteToFile();
            AppLib.WPF.Dialogs.Dialogs.ShowErrorDialog(e.Exception);
        }
    }
}
