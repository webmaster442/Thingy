using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.CoreModules.ModuleDefinitons
{
    public class XKCDReaderModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "XKCD Reader"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/xkcd.png"); }
        }

        public override Color TileColor
        {
            get { return Color.FromRgb(230, 230, 230); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.XKCDReader
            {
                DataContext = new ViewModels.XKCDReaderViewModel(App)
            };
        }
    }
}
