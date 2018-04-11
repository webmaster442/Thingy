using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Modules
{
    public class CurrencyConverterModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Currency Converter"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-currency-exchange-96.png"); }
        }

        public override UserControl RunModule()
        {
            return new CoreModules.Views.CurrencyConverter
            {
                DataContext = new CoreModules.ViewModels.CurrencyConverterViewModel()
            };
        }

        public override string Category
        {
            get { return ModuleCategories.Calculators; }
        }
    }
}
