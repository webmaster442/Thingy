using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Calculator.ModuleDefinitions
{
    public class CalculatorModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Calculator"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-calculator-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Calculators; }
        }

        public override UserControl RunModule()
        {
            var view = new Views.Calculator();
            view.DataContext = new ViewModels.CalculatorViewModel(view, App);
            return view;
        }
    }
}
