using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
using Thingy.Db;

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
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-folder-tree.png"); }
        }

        public override UserControl RunModule()
        {
            return new CoreModules.Views.Places
            {
                DataContext = new CoreModules.ViewModels.PlacesViewModel(App, App.Resolve<IDataBase>())
            };
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
