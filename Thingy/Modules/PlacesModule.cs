using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Db;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class PlacesModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Places"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-folder-tree.png")); }
        }

        public override UserControl RunModule()
        {
            return new Views.Places
            {
                DataContext = new ViewModels.PlacesViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
