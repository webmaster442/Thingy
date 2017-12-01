using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thingy.Modules
{
    public class CommandLineModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Command Line"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-console.png")); }
        }

        public override UserControl RunModule()
        {
            var view = new Views.CommandLine();
            view.DataContext = new ViewModels.CommandLineViewModel(view);
            return view;
        }
    }
}
