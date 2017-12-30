using AppLib.MVVM;
using System;
using System.Collections.Generic;

namespace Thingy.Db.Entity
{
    public class Alarm: ValidatableBase, IEquatable<Alarm>
    {
        private long? _dueDate;
        private string _Description;
        private bool _active;

        public long? DueDate
        {
            get { return _dueDate; }
            set { SetValue(ref _dueDate, value); }
        }

        public string Description
        {
            get { return _Description; }
            set { SetValue(ref _Description, value); }
        }

        public bool Active
        {
            get { return _active; }
            set { SetValue(ref _active, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Alarm);
        }

        public bool Equals(Alarm other)
        {
            return other != null &&
                   EqualityComparer<long?>.Default.Equals(_dueDate, other._dueDate) &&
                   _Description == other._Description &&
                   _active == other._active;
        }

        public override int GetHashCode()
        {
            var hashCode = -198705488;
            hashCode = hashCode * -1521134295 + EqualityComparer<long?>.Default.GetHashCode(_dueDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_Description);
            hashCode = hashCode * -1521134295 + _active.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Alarm alarm1, Alarm alarm2)
        {
            return EqualityComparer<Alarm>.Default.Equals(alarm1, alarm2);
        }

        public static bool operator !=(Alarm alarm1, Alarm alarm2)
        {
            return !(alarm1 == alarm2);
        }
    }
}
