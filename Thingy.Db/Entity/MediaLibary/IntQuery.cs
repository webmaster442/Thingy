using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity.MediaLibary
{
    public enum IntOperator : int
    {
        [Display(Name = "=")]
        Equals = 0,
        [Display(Name = "<")]
        Less = 1,
        [Display(Name = "<=")]
        LessOrEqual = 2,
        [Display(Name = ">")]
        Greater = 3,
        [Display(Name = ">=")]
        GreaterOrEqual = 4
    }

    public class IntQuery : BindableBase, IEquatable<IntQuery>
    {
        private IntOperator _operator;
        private int? _value;
        private bool _HasValue;

        public IntOperator Operator
        {
            get { return _operator; }
            set { SetValue(ref _operator, value); }
        }

        public int? Value
        {
            get { return _value; }
            set
            {
                SetValue(ref _value, value);
                HasValue = value != null;
            }
        }

        public IntQuery()
        {
            Operator = IntOperator.Equals;
            Value = null;
        }

        public IntQuery(int value, IntOperator op)
        {
            Operator = op;
            Value = value;
        }

        public bool HasValue
        {
            get { return _HasValue; }
            set
            {
                if (value == false && Value != null) Value = null;
                SetValue(ref _HasValue, value);
            }
        }


        public bool IsMatch(int? other)
        {
            if (other == null && Value == null) return true;
            if (other == null || Value == null) return false;

            switch (Operator)
            {
                case IntOperator.Equals:
                    return Value.Value == other.Value;
                case IntOperator.Greater:
                    return Value.Value > other.Value;
                case IntOperator.GreaterOrEqual:
                    return Value.Value >= other.Value;
                case IntOperator.Less:
                    return Value.Value < other.Value;
                case IntOperator.LessOrEqual:
                    return Value.Value <= other.Value;
                default:
                    return false;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IntQuery);
        }

        public bool Equals(IntQuery other)
        {
            return other != null &&
                   Operator == other.Operator &&
                   EqualityComparer<int?>.Default.Equals(Value, other.Value) &&
                   HasValue == other.HasValue;
        }

        public override int GetHashCode()
        {
            var hashCode = -1404276345;
            hashCode = hashCode * -1521134295 + Operator.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + HasValue.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(IntQuery query1, IntQuery query2)
        {
            return EqualityComparer<IntQuery>.Default.Equals(query1, query2);
        }

        public static bool operator !=(IntQuery query1, IntQuery query2)
        {
            return !(query1 == query2);
        }
    }
}
