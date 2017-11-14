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
        public ObservableCollection<HeaderedItemViewModel> Items;

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<HeaderedItemViewModel>();
            Items.Add(ItemFactory.Invoke());
        }

        public Func<HeaderedItemViewModel> ItemFactory
        {
            get
            {
                return () =>
                {
                    var start = new StartPage();
                    start.DataContext = new StartPageViewModel(App.IoCContainer.ResolveSingleton<IModuleLoader>());

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
