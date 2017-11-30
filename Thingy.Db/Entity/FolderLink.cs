using AppLib.MVVM;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity
{
    public class FolderLink: ValidatableBase, IEquatable<FolderLink>
    {
        private string _name;
        private string _path;

        public FolderLink()
        {
            ValidateOnPropertyChange = true;
            Validate();
        }

        [BsonId]
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        [BsonField]
        [Required]
        [CustomValidation(typeof(FolderLink), "PathValidate")]
        public string Path
        {
            get { return _path; }
            set { SetValue(ref _path, value); }
        }

        public static ValidationResult PathValidate(object obj, ValidationContext context)
        {
            var folderLink = (FolderLink)context.ObjectInstance;
            if (!System.IO.Directory.Exists(folderLink.Path))
            {
                return new ValidationResult(string.Format("Path doesn't exist: {0}", folderLink.Path),
                                            new string[] { nameof(folderLink.Path) });
            }
            return ValidationResult.Success;
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
