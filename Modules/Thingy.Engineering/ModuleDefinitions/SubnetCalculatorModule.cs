using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Engineering.ModuleDefinitions
{
    public class SubnetCalculatorModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Subnet Calculator"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-wired-network-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Science; }
        }

        public override UserControl RunModule()
        {
            return new Views.SubnetCalculator
            {
                DataContext = new ViewModels.SubnetCalculatorViewModel()
            };
        }
    }
}
