using AppLib.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.CoreModules.ModuleDefinitons
{
    public class LinuxSubSystemModule: ModuleBase
    {
        private const string path = @"%windir%\system32\bash.exe";

        public override string ModuleName
        {
            get { return "Linux Subsystem for Windows"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-linux-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }

        public override bool CanLoad
        {
            get
            {
                return System.IO.File.Exists(Environment.ExpandEnvironmentVariables(path));
            }
        }

        public override UserControl RunModule()
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = Environment.ExpandEnvironmentVariables(path);
            p.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            p.Start();
            return null;
        }
    }
}
