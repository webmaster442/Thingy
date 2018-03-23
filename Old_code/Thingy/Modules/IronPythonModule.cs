using AppLib.WPF;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class IronPythonModule : ModuleBase
    {
        private string _thingycmd;

        public override string ModuleName
        {
            get { return "Iron Python"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-python-96.png"); }
        }

        public override UserControl RunModule()
        {
            var ipy = new System.Diagnostics.Process();
            ipy.StartInfo.FileName = _thingycmd;
            ipy.StartInfo.Arguments = "ipy";
            ipy.Start();
            return null;
        }

        public override bool CanLoad
        {
            get
            {
                var app = AppDomain.CurrentDomain.BaseDirectory;
                _thingycmd = Path.Combine(app, @"Thingy.Cmd.exe");
                return File.Exists(_thingycmd);
            }
        }

        public override string Category
        {
            get { return ModuleCategories.CommandLine; }
        }
    }
}
