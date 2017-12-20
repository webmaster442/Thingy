﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thingy.Modules
{
    public class IronPythonModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Iron Python"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-python-96.png")); }
        }

        public override UserControl RunModule()
        {
            return new IronPython.IronPythonShell();
        }
    }
}