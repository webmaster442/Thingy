﻿using System;
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

    public class IntQuery : IEquatable<IntQuery>
    {
        public IntOperator Operator { get; set; }
        public int? Value { get; set; }

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
            get { return Value != null; }
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
