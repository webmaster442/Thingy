using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Infrastructure;

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
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-powershell.png")); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override UserControl RunModule()
        {
            var view = new Views.CommandLine();
            view.DataContext = new ViewModels.CommandLineViewModel(view, "powershell.exe");
            return view;
        }
    }
}
