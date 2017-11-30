using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Thingy.Infrastructure;
using Thingy.Modules;
using Thingy.Views;

namespace Thingy.ViewModels
{
    public class StartPageViewModel: ViewModel
    {
        private IModuleLoader _moduleLoader;
        private IApplication _application;

        public DelegateCommand<string> TileClickCommand { get; private set; }

        public StartPageViewModel(IApplication application, IModuleLoader moduleLoader)
        {
            _moduleLoader = moduleLoader;
            _application = application;
            Modules = new ObservableCollection<IModule>();
            Modules.AddRange(_moduleLoader.Modules);
            TileClickCommand = DelegateCommand<string>.ToCommand(TileClick);
        }

        private void TileClick(string obj)
        {
            var control = _moduleLoader.GetModuleByName(obj);
            _application.SetCurrentTabContent(obj, control);

        }

        public ObservableCollection<IModule> Modules
        {
            get;
            private set;
        }
    }
}
