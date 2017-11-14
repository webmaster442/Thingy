using AppLib.Common.Extensions;
using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Thingy.Infrastructure;
using Thingy.Modules;

namespace Thingy.ViewModels
{
    public class StartPageViewModel: ViewModel
    {
        private IModuleLoader _moduleLoader;

        public DelegateCommand TileClickCommand { get; private set; }

        public StartPageViewModel(IModuleLoader moduleLoader)
        {
            _moduleLoader = moduleLoader;
            Modules = new ObservableCollection<IModule>();
            Modules.AddRange(_moduleLoader);
            TileClickCommand = DelegateCommand.ToCommand(TileClick);
        }

        private void TileClick()
        {
            MessageBox.Show("Blah");
        }

        public ObservableCollection<IModule> Modules
        {
            get;
            private set;
        }
    }
}
