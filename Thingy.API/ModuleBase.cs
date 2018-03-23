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

        public virtual bool OpenAsWindow
        {
            get { return false; }
        }

        public virtual bool IsSingleInstance
        {
            get { return false; }
        }

        public virtual IEnumerable<string> SupportedExtensions
        {
            get { return null; }
        }
    }
}
