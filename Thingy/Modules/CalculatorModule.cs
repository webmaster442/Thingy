using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class CalculatorModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Calculator"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-calculator-96.png")); }
        }

        public override string Category
        {
            get { return ModuleCategories.Calculators; }
        }

        public override UserControl RunModule()
        {
            var view = new Views.Calculator();
            view.DataContext = new ViewModels.CalculatorViewModel(view);
            return view;
        }
    }
}
