using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
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
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-folder-tree.png"); }
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
