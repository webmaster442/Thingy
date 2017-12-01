﻿using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Implementation;

namespace Thingy.Modules
{
    public abstract class ModuleBase : IModule
    {
        public abstract string ModuleName { get; }

        public abstract ImageSource Icon { get; }

        public abstract UserControl RunModule();

        public virtual Color TileColor
        {
            get
            {
                var nameHash = ModuleName.GetHashCode();
                return TileColorProvider.GetColor(nameHash);
            }
        }

        public virtual bool CanLoad
        {
            get { return true; }
        }

        public SolidColorBrush ColorBrush
        {
            get { return new SolidColorBrush(TileColor); }
        }
    }
}
