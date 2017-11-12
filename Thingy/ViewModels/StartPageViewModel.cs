using AppLib.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.Infrastructure;
using Thingy.Modules;

namespace Thingy.ViewModels
{
    public class StartPageViewModel
    {
        private IModuleLoader _moduleLoader;

        public StartPageViewModel(IModuleLoader moduleLoader)
        {
            _moduleLoader = moduleLoader;
            Modules = new ObservableCollection<AbstractModule>();
            Modules.AddRange(_moduleLoader);
        }

        public ObservableCollection<AbstractModule> Modules
        {
            get;
            private set;
        }
    }
}
