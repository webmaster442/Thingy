using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class IronPythonModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Iron Python"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-python-96.png")); }
        }

        public override UserControl RunModule()
        {
            return null;
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }
    }
}
