using AppLib.Common.Extensions;
using System;
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

    public class StringQuery
    {
        public StringOperator Operator { get; set; }
        public string Value { get; set; }

        public StringQuery()
        {
            Operator = StringOperator.ContainsIgnoreCase;
            Value = null;
        }

        public bool HasValue
        {
            get { return !string.IsNullOrEmpty(Value); }
        }

        private bool IsMatch(string other)
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
    }
}
