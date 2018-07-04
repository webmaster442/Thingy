using AppLib.WPF;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.GitBash.ModuleDefinitions
{
    public class GitBashModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Git Bash"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-git.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override UserControl RunModule()
        {
            var view = new Views.GitBashView();
            view.DataContext = new ViewModels.GitBashViewModel(App, view);
            return view;
        }

        public override bool CanLoad
        {
            get
            {
                var pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var _gitPath = Path.Combine(pf, @"Git\git-bash.exe");
                return File.Exists(_gitPath);
            }
        }

        public override bool CanHadleFile(string pathOrExtension)
        {
            return Directory.Exists(pathOrExtension);
        }

        public override bool SupportsFolderAsArgument
        {
            get { return true; }
        }
    }
}
