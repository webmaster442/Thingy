﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thingy.Modules
{
    public class PlacesModule : IModule
    {
        public string ModuleName
        {
            get { return "Places"; }
        }

        public ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy;component/Icons/icons8-folder-tree.png")); }
        }

        public UserControl RunModule()
        {
            return new Views.Places
            {
                DataContext = new ViewModels.PlacesViewModel()
            };
        }
    }
}
