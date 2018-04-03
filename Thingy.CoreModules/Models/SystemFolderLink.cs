using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Thingy.CoreModules.Models
{
    public class SystemFolderLink : BindableBase, IEquatable<SystemFolderLink>
    {
        private string _name;
        private string _path;
        private BitmapImage _icon;


        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string Path
        {
            get { return _path; }
            set { SetValue(ref _path, value); }
        }

        public BitmapImage Icon
        {
            get { return _icon; }
            set { SetValue(ref _icon, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SystemFolderLink);
        }

        public bool Equals(SystemFolderLink other)
        {
            return other != null &&
                   _name == other._name &&
                   _path == other._path &&
                   EqualityComparer<BitmapImage>.Default.Equals(_icon, other._icon);
        }

        public override int GetHashCode()
        {
            var hashCode = 918483829;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_path);
            hashCode = hashCode * -1521134295 + EqualityComparer<BitmapImage>.Default.GetHashCode(_icon);
            return hashCode;
        }

        public static bool operator ==(SystemFolderLink link1, SystemFolderLink link2)
        {
            return EqualityComparer<SystemFolderLink>.Default.Equals(link1, link2);
        }

        public static bool operator !=(SystemFolderLink link1, SystemFolderLink link2)
        {
            return !(link1 == link2);
        }
    }
}
