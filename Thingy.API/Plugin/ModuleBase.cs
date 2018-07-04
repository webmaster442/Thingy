using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API.Internals;

namespace Thingy.API
{
    public abstract class ModuleBase : IModule
    {
        public abstract string ModuleName { get; }

        public abstract ImageSource Icon { get; }

        public abstract UserControl RunModule();

        public virtual void AppAttached()
        {
        }

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

        public abstract string Category { get; }

        public virtual bool IsSingleInstance
        {
            get { return false; }
        }

        public IApplication App { get; set; }

        public virtual OpenParameters OpenParameters
        {
            get { return null; }
        }

        public virtual bool CanHadleFile(string pathOrExtension)
        {
            return false;
        }
    }
}
