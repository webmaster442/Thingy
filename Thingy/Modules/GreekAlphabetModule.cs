using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-alpha-96.png")); }
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
