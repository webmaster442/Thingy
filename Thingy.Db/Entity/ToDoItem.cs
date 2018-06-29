using AppLib.MVVM;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity
{
    public class ToDoItem : ValidatableBase, IEquatable<ToDoItem>
    {
        private string _content;
        private bool _iscompleted;
        private DateTime? _completeddate;
        private DateTime? _duedate;

        public ToDoItem()
        {
            ValidateOnPropertyChange = true;
            Validate();
        }

        [BsonId]
        [Required]
        public string Name
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }

        [BsonField]
        public bool IsCompleted
        {
            get { return _iscompleted; }
            set
            {
                SetValue(ref _iscompleted, value);
                if (CompletedDate == null && IsCompleted)
                    CompletedDate = DateTime.Now;
            }
        }

        [BsonField]
        public DateTime? CompletedDate
        {
            get { return _completeddate; }
            set { SetValue(ref _completeddate, value); }
        }

        [BsonField]
        public DateTime? DueDate
        {
            get { return _duedate; }
            set { SetValue(ref _duedate, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ToDoItem);
        }

        public bool Equals(ToDoItem other)
        {
            return other != null &&
                   Name == other.Name &&
                   IsCompleted == other.IsCompleted &&
                   EqualityComparer<DateTime?>.Default.Equals(CompletedDate, other.CompletedDate) &&
                   EqualityComparer<DateTime?>.Default.Equals(DueDate, other.DueDate);
        }

        public override int GetHashCode()
        {
            var hashCode = 1682209475;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + IsCompleted.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(CompletedDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(DueDate);
            return hashCode;
        }

        public static bool operator ==(ToDoItem item1, ToDoItem item2)
        {
            return EqualityComparer<ToDoItem>.Default.Equals(item1, item2);
        }

        public static bool operator !=(ToDoItem item1, ToDoItem item2)
        {
            return !(item1 == item2);
        }
    }
}
