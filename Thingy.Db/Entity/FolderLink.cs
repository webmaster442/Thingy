using AppLib.WPF.MVVM;
using LiteDB;
using System;
using System.Collections.Generic;

namespace Thingy.Db.Entity
{
    public class FolderLink: BindableBase, IEquatable<FolderLink>
    {
        private string _name;
        private string _path;

        [BsonId]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        [BsonField]
        public string Path
        {
            get { return _path; }
            set { SetValue(ref _path, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FolderLink);
        }

        public bool Equals(FolderLink other)
        {
            return other != null &&
                   _name == other._name &&
                   _path == other._path;
        }

        public override int GetHashCode()
        {
            var hashCode = -827305254;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_path);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Path);
        }

        public static bool operator ==(FolderLink link1, FolderLink link2)
        {
            return EqualityComparer<FolderLink>.Default.Equals(link1, link2);
        }

        public static bool operator !=(FolderLink link1, FolderLink link2)
        {
            return !(link1 == link2);
        }
    }
}
