using AppLib.MVVM;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Db.Entity
{
    public class Note: ValidatableBase, IEquatable<Note>
    {
        private string _name;
        private string _content;

        public Note()
        {
            ValidateOnPropertyChange = true;
            Validate();
        }

        [Required]
        [BsonId]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        [BsonField]
        public string Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Note);
        }

        public bool Equals(Note other)
        {
            return other != null &&
                   _name == other._name &&
                   _content == other._content;
        }

        public override int GetHashCode()
        {
            var hashCode = -1106603996;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_content);
            return hashCode;
        }

        public static bool operator ==(Note note1, Note note2)
        {
            return EqualityComparer<Note>.Default.Equals(note1, note2);
        }

        public static bool operator !=(Note note1, Note note2)
        {
            return !(note1 == note2);
        }
    }
}
