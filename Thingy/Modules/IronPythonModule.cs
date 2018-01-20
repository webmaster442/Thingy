using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class IronPythonModule : ModuleBase
    {
        private string _ipy;

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
            var view = new Views.CommandLine();
            view.DataContext = new ViewModels.CommandLineViewModel(view, _ipy, "ipy");
            return view;
        }

        public override bool CanLoad
        {
            get
            {
                var app = AppDomain.CurrentDomain.BaseDirectory;
                _ipy = Path.Combine(app, @"Thingy.Cmd.exe");
                return File.Exists(_ipy);
            }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }
    }
}
