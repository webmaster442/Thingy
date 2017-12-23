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

        public MainWindowViewModel()
        {
            SettingCommand = Command.ToCommand(Setting, CanOpenSetting);
            ExitCommand = Command.ToCommand(Exit);
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

        public ItemActionCallback ClosingTabItemHandler
        {
            get { return ClosingTabItemHandlerImpl; }
        }

        private static void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            var viewModel = args.DragablzItem.DataContext as HeaderedItemViewModel;
            if (viewModel.Content is IDisposable disposable)
            {
                disposable.Dispose();
            }
            viewModel.Content = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
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
                        Header = "New Tool...",
                        Content = start
                    };
                };
            }
        }
    }
}
