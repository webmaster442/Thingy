using AppLib.WPF.MVVM;
using Dragablz;
using System;
using System.Collections.ObjectModel;
using Thingy.Infrastructure;
using Thingy.ViewModels;
using Thingy.Views;

namespace Thingy
{
    public class MainWindowViewModel: ViewModel
    {
        public MainWindowViewModel()
        {
        }

        public Func<HeaderedItemViewModel> ItemFactory
        {
            get
            {
                return () =>
                {
                    var start = new StartPage();
                    start.DataContext = new StartPageViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IModuleLoader>());

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
