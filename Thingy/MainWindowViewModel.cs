using AppLib.Common.Log;
using AppLib.MVVM;
using Dragablz;
using System;
using Thingy.Infrastructure;
using Thingy.ViewModels;
using Thingy.Views;

namespace Thingy
{
    public class MainWindowViewModel: ViewModel
    {
        public DelegateCommand SettingCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand LogCommand { get; set; }

        private ILogger _log;
        private IApplication _app;

        public MainWindowViewModel(IApplication app, ILogger log)
        {
            _app = app;
            _log = log;
            SettingCommand = Command.ToCommand(Setting, CanOpenSetting);
            ExitCommand = Command.ToCommand(Exit);
            LogCommand = Command.ToCommand(Log);
        }

        private void Log()
        {
            var logviewer = new AppLib.WPF.Dialogs.LogViewer
            {
                Log = _log
            };
            _app.ShowDialog(logviewer, "Application Log");
        }

        private bool CanOpenSetting()
        {
            int index = _app.FindTabByTitle("Settings");
            return index == -1;
        }

        private void Setting()
        {
            _app.OpenTabContent("Settings", new Views.Settings());
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        public Func<HeaderedItemViewModel> ItemFactory
        {
            get
            {
                return () =>
                {
                    var start = new StartPage
                    {
                        DataContext = new StartPageViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IModuleLoader>())
                    };

                    return new HeaderedItemViewModel
                    {
                        IsSelected = true,
                        Header = "New Tool...",
                        Content = start
                    };
                };
            }
        }
    }
}
