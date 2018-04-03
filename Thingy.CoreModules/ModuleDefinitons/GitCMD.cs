using AppLib.WPF;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Modules
{
    public class GitCMD : ModuleBase
    {
        private string _gitPath;

        public override string ModuleName
        {
            get { return "GIT CMD"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-git.png"); }
        }

        public override UserControl RunModule()
        {
            var view = new CoreModules.Views.CommandLine();
            view.DataContext = new CoreModules.ViewModels.CommandLineViewModel(view, _gitPath);
            return view;
        }

        public override bool CanLoad
        {
            get
            {
                var pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                _gitPath = Path.Combine(pf, @"Git\git-cmd.exe");
                return File.Exists(_gitPath);
            }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

    }
}
