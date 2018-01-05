using AppLib.Common.Log;
using AppLib.MVVM;
using Dragablz;
using Dragablz.Dockablz;
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

        public MainWindowViewModel()
        {
            SettingCommand = Command.ToCommand(Setting, CanOpenSetting);
            ExitCommand = Command.ToCommand(Exit);
            LogCommand = Command.ToCommand(Log);
        }

        private void Log()
        {
            var logviewer = new AppLib.WPF.Dialogs.LogViewer();
            logviewer.Log = App.IoCContainer.ResolveSingleton<ILogger>();
            App.Instance.ShowDialog(logviewer, "Application Log");
        }

        private bool CanOpenSetting()
        {
            int index = App.Instance.FindTabByTitle("Settings");
            return index == -1;
        }

        private void Setting()
        {
            App.Instance.OpenTabContent("Settings", new Views.Settings());
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
