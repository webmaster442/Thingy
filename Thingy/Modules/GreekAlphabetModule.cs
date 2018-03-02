using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class GreekAlphabetModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Greek Alphabet"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-alpha-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Science; }
        }

        public override UserControl RunModule()
        {
            return new Views.GreekAlphabet();
        }
    }
}
