using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.IronPythonConsole.ModuleDefinitions
{
    public class IronPythonConsoleModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "IronPython"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-python-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override UserControl RunModule()
        {
            return new Views.IronPythonConsole(App);
        }
    }
}
