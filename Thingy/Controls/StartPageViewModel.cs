using AppLib.Common.Extensions;
using AppLib.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Thingy.API;
using Thingy.Implementation;

namespace Thingy.Controls
{
    internal class StartPageViewModel: ViewModel
    {
        private IModuleLoader _moduleLoader;
        private IApplication _application;

        private string _searchname;
        private int _categoryIndex;

        public DelegateCommand<string> TileClickCommand { get; private set; }
        public DelegateCommand<KeyValuePair<string, int>> FilterCategoryCommand { get; private set; }

        public StartPageViewModel(IApplication application, IModuleLoader moduleLoader)
        {
            CategoryIndex = 0;
            _moduleLoader = moduleLoader;
            _application = application;
            Modules = new ObservableCollection<IModule>();
            Modules.AddRange(_moduleLoader.GetModulesForCategory());
            TileClickCommand = Command.CreateCommand<string>(TileClick);
            Categories = new Dictionary<string, int>(_moduleLoader.CategoryModuleCount);
            FilterCategoryCommand = Command.CreateCommand<KeyValuePair<string, int>>(FilterCategory);
        }

        private void FilterCategory(KeyValuePair<string, int> obj)
        {
            SearchName = string.Empty;
            Modules.UpdateWith(_moduleLoader.GetModulesForCategory(obj.Key));
        }

        private async void TileClick(string obj)
        {
            var module = _moduleLoader.GetModuleByName(obj);
            await _application.TabManager.StartModule(module);
        }

        public ObservableCollection<IModule> Modules
        {
            get;
            private set;
        }

        public Dictionary<string, int> Categories
        {
            get;
            private set;
        }

        public string SearchName
        {
            get { return _searchname; }
            set
            {
                if (SetValue(ref _searchname, value))
                {
                    CategoryIndex = 0;
                    Modules.UpdateWith(_moduleLoader.GetModulesByName(_searchname));
                }
            }
        }

        public int CategoryIndex
        {
            get { return _categoryIndex; }
            set { SetValue(ref _categoryIndex, value); }
        }
    }
}
