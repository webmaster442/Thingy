﻿using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class RunProgramModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Run Program"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-run-command-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.RunProgram(App.Log);
        }

        public override bool OpenAsWindow
        {
            get { return true; }
        }
    }
}