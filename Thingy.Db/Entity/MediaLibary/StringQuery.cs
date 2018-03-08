using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Thingy.Db.Entity.MediaLibary
{
    public enum StringOperator : int
    {
        [Display(Name = "Contains, Ignore Case")]
        ContainsIgnoreCase = 0,
        [Display(Name = "Contains")]
        Contains = 1,
        [Display(Name = "Exact Match, Ignore Case")]
        ExactmatchIgnoreCase = 2,
        [Display(Name = "Exact Match")]
        Exactmatch = 3
    }

    public class StringQuery : BindableBase, IEquatable<StringQuery>
    {
        private string _value;
        private StringOperator _operator;

        public StringOperator Operator
        {
            get { return _operator; }
            set { SetValue(ref _operator, value); }
        }

        public string Value
        {
            get { return _value; }
            set { SetValue(ref _value, value); }
        }

        public StringQuery()
        {
            Operator = StringOperator.ContainsIgnoreCase;
            Value = null;
        }

        public StringQuery(string value, StringOperator op)
        {
            Value = value;
            Operator = op;
        }

        public bool HasValue
        {
            get { return !string.IsNullOrEmpty(Value); }
            set
            {
                Value = null;
                OnPropertyChanged();
            }
        }

        public bool IsMatch(string other)
        {
            if (other == null && Value == null) return true;
            if (other == null || Value == null) return false;

            switch (Operator)
            {
                case StringOperator.Contains:
                    return other.Contains(Value);
                case StringOperator.ContainsIgnoreCase:
                    return other.Contains(Value, StringComparison.InvariantCultureIgnoreCase);
                case StringOperator.Exactmatch:
                    return other == Value;
                case StringOperator.ExactmatchIgnoreCase:
                    return String.Compare(other, Value, true) == 0;
                default:
                    return false;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StringQuery);
        }

        public bool Equals(StringQuery other)
        {
            return other != null &&
                   Operator == other.Operator &&
                   Value == other.Value &&
                   HasValue == other.HasValue;
        }

        public override int GetHashCode()
        {
            var hashCode = -1404276345;
            hashCode = hashCode * -1521134295 + Operator.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + HasValue.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StringQuery query1, StringQuery query2)
        {
            return EqualityComparer<StringQuery>.Default.Equals(query1, query2);
        }

        public static bool operator !=(StringQuery query1, StringQuery query2)
        {
            return !(query1 == query2);
        }
    }
}
