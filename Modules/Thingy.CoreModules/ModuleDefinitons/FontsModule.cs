﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.API;
using Thingy.CoreModules.Views;

namespace Thingy.Modules
{
    public class FontsModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Fonts"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-choose-font-96.png")); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new FontViewer
            {
                DataContext = new CoreModules.ViewModels.FontViewerViewModel(App)
            };
        }
    }
}
