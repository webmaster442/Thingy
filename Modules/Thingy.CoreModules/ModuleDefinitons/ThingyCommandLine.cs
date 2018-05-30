using AppLib.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.CoreModules.ModuleDefinitons
{
    public class ThingyCommandLine: ModuleBase
    {
        public override string ModuleName
        {
            get { return "Thingy Command Line"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-swiss-army-knife-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override UserControl RunModule()
        {
            var view = new CoreModules.Views.CommandLine();
            view.DataContext = new CoreModules.ViewModels.CommandLineViewModel(view, "Thingy.cmd.exe");
            return view;
        }
    }
}
