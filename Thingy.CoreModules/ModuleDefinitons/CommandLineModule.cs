using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
using Thingy.CoreModules.Views;

namespace Thingy.Modules
{
    public class CommandLineModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Windows Command Line"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-console.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override UserControl RunModule()
        {
            var view = new CommandLine();
            view.DataContext = new ViewModels.CommandLineViewModel(view);
            return view;
        }
    }
}
