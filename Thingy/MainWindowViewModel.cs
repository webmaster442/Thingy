using AppLib.Common.Log;
using AppLib.MVVM;
using Dragablz;
using System;
using Thingy.Infrastructure;

namespace Thingy
{
    public class MainWindowViewModel: ViewModel<IMainWindow>
    {
        public DelegateCommand SettingCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand OpenMenuCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }

        private ILogger _log;
        private IApplication _app;

        public MainWindowViewModel(IMainWindow view, IApplication app, ILogger log): base(view)
        {
            _app = app;
            _log = log;
            SettingCommand = Command.ToCommand(Setting, CanOpenSetting);
            ExitCommand = Command.ToCommand(Exit);
            LogCommand = Command.ToCommand(Log);
            OpenMenuCommand = Command.ToCommand(OpenMenu);
            AboutCommand = Command.ToCommand(OpenAbout);
        }

        private void OpenAbout()
        {
            _app.ShowTabContent("About", new Views.About());
        }

        private void OpenMenu()
        {
            View.ShowMenu();
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
            _app.ShowTabContent("Settings", new Views.Settings());
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
                    var start = new Views.StartPage
                    {
                        DataContext = new ViewModels.StartPageViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IModuleLoader>())
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
