using AppLib.MVVM;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity
{
    public class VirualFolder: ValidatableBase, IEquatable<VirualFolder>
    {
        private string _FolderName;
        private List<string> _files;

        public VirualFolder()
        {
            ValidateOnPropertyChange = true;
        }

        [BsonId]
        [Required]
        public string Name
        {
            get { return _FolderName; }
            set { SetValue(ref _FolderName, value); }
        }

        [BsonField]
        public List<string> Files
        {
            get { return _files; }
            set { SetValue(ref _files, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VirualFolder);
        }

        public bool Equals(VirualFolder other)
        {
            return other != null &&
                   _FolderName == other._FolderName &&
                   EqualityComparer<List<string>>.Default.Equals(_files, other._files);
        }

        public override int GetHashCode()
        {
            var hashCode = -873061146;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_FolderName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(_files);
            return hashCode;
        }

        public static bool operator ==(VirualFolder folder1, VirualFolder folder2)
        {
            return EqualityComparer<VirualFolder>.Default.Equals(folder1, folder2);
        }

        public static bool operator !=(VirualFolder folder1, VirualFolder folder2)
        {
            return !(folder1 == folder2);
        }
    }
}
