using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Modules
{
    class PowerShellModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Powershell"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-powershell.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override UserControl RunModule()
        {
            var view = new CoreModules.Views.CommandLine();
            view.DataContext = new ViewModels.CommandLineViewModel(view, "powershell.exe");
            return view;
        }
    }
}
