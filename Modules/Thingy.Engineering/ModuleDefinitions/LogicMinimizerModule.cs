using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Engineering.ModuleDefinitions
{
    public class LogicMinimizerModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Logic function minimizer"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-data-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Science; }
        }

        public override UserControl RunModule()
        {
            return new Views.LogicFunctionMinimizer(App);
        }
    }
}